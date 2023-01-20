namespace FileCabinet.NET.C_.Utils;

public class NginxChecker
{
    public string Host { get; }
    public PlatformID HostOs { get; }

    private string _linUserLogin => _settings.CKDomain.LinuxUser.Login;
    private string _linUserPass => _settings.CKDomain.LinuxUser.Password;
    private TimeSpan _defaultTimeout => _settings.DefaultTimeout;

    private readonly ILogger _logger;
    private readonly SettingsModel _settings;

    public NginxChecker(SettingsModel settings, string host, ILogger logger)
    {
        Host = host;
        _settings = settings;
        using var remotefs = CommonTools.GetRemoteFileSystem(Host, logger, _linUserLogin, _linUserPass);
        HostOs = remotefs.Platform;

        var prefixLastValue = HostOs == PlatformID.Unix ? "SSH" : "PS";
        _logger = new PrefixLogger(logger, $"[{nameof(NginxChecker)}][{prefixLastValue}] ");
    }

    public bool Check()
    {
        if (HostOs == PlatformID.Unix)
        {
            using var unixProvider = new UnixProvider(Host, _linUserLogin, _linUserPass, _logger);

            var exitCode = ExecuteSshCmd(unixProvider, "sudo nginx -t");
            return exitCode == 0;
        }
        else
        {
            return ExecutePsCmd_CheckNginx(Host);
        }
    }

    public int ExecuteSshCmd(UnixProvider unixProvider, string text)
    {
        _logger.Debug($"Выполняю: '{text}'");
        using (SshCommand sshCommand = unixProvider.Ssh.CreateCommand(text))
        {
            sshCommand.CommandTimeout = _defaultTimeout;
            var outputText = sshCommand.Execute();

            _logger.Debug($"Результат '{text}': {Environment.NewLine}ExitStatus: {sshCommand.ExitStatus}" +
                                              $"{Environment.NewLine}StdOut:{Environment.NewLine}{outputText.Trim()}" +
                                              $"{Environment.NewLine}StdErr:{Environment.NewLine}{sshCommand.Error.Trim()}");
            return sshCommand.ExitStatus;
        }
    }

    public bool ExecutePsCmd_CheckNginx(string remoteServerName)
    {
        var nginxPath = @"nginx.exe";
        var nginxArgs = "-t";
        var workingDir = @"c:\Program Files\nginx\";
        return ExecutePsCmd(remoteServerName, nginxPath, nginxArgs, workingDir);
    }

    public bool ExecutePsCmd(string remoteServerName, string processName, string args, string workingDir)
    {
        var psScript = CreatePsScript(remoteServerName, processName, args, workingDir, out var exitCodePrefix, out var outputCodePrefix);

        var outString = ExecutePsScript(psScript);
        _logger.Debug($"Результат выполнения скрипта: {outString.Trim()}");

        var parsedPsResult = ParsePsOutput(outString, exitCodePrefix, outputCodePrefix);

        if (parsedPsResult.IsSuccessParsed)
        {
            _logger.Trace($"Результат выполнения скрипта успешно распаршен в:{Environment.NewLine}" +
                $"{JsonConvert.SerializeObject(parsedPsResult, Formatting.Indented)}");

            return parsedPsResult.ExitCode == 0;
        }
        else
        {
            _logger.Error($"Output скрипта не был успешно распаршен");
            return false;
        }
    }

    public record PsScriptResult()
    {
        public bool IsSuccessParsed { get; set; }
        public int ExitCode { get; set; }
        public string OutputLog { get; set; }
    }

    public PsScriptResult ParsePsOutput(string outString, string exitCodePrefix, string outputCodePrefix)
    {
        var result = new PsScriptResult();
        try
        {
            var outputLines = SplitByNewLinesChars(outString);

            {
                var exitCodeFullStr = outputLines.First();
                var exitCodeStr = exitCodeFullStr.Replace(exitCodePrefix, string.Empty).Trim();
                var exitCode = int.Parse(exitCodeStr);
                result.ExitCode = exitCode;
                outputLines.Remove(exitCodeFullStr);
            }

            {
                var outputLine = outputLines.First();
                if (!outputLine.Contains(outputCodePrefix))
                    throw new Exception($"Вторая строка не содержала префикса: {outputCodePrefix}");
                outputLines.Remove(outputCodePrefix);

                var outputLog = outputLines.Aggregate((f, s) => $"{f}{Environment.NewLine}{s}");
                result.OutputLog = outputLog.Trim();
            }

            result.IsSuccessParsed = true;
            return result;
        }
        catch (Exception exc)
        {
            _logger.Error($"Не получилось распарсить выходную строку из PS скрипта: {exc}");
            result.IsSuccessParsed = false;
            return result;
        }
    }

    private List<string> SplitByNewLinesChars(string outString)
    {
        var result = new List<string>();
        using (var reader = new StringReader(outString))
        {
            string line;
            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                result.Add(line);
            }
        }

        return result;
    }

    public string ExecutePsScript(string psScript)
    {
        var tempPath = Path.GetTempPath();
        var sriptpath = Path.Combine(tempPath, Guid.NewGuid().ToString() + ".ps1");
        try
        {
            using (var stream = File.Create(sriptpath))
            using (var sw = new StreamWriter(stream))
                sw.Write(psScript);

            _logger.Trace($"Выполняю скрипт: '{psScript}'");

            return ExecutePsFileScript(sriptpath);
        }
        catch (Exception exc)
        {
            _logger.Error($"Ошибка во время исполнения PS скрипта: {exc}");
            return null;
        }
        finally
        {
            if (File.Exists(sriptpath))
                File.Delete(sriptpath);
        }
    }

    public string CreatePsScript(string remoteServerName, string processName
        , string args, string workingDir
        , out string exitCodePrefix, out string outputCodePrefix)
    {
        exitCodePrefix = "Exitcode:";
        outputCodePrefix = "Output:";

        var fullProcessName = Path.IsPathFullyQualified(processName)
            ? processName
            : Path.Combine(workingDir, processName);

        return $@"
        Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
        $serverName = ""{remoteServerName}""
        $command = {{ 
            $pinfo = New-Object System.Diagnostics.ProcessStartInfo
            $pinfo.FileName = ""{fullProcessName}"" 
            $pinfo.Arguments = ""{args}""
            $pinfo.RedirectStandardError = $true
            $pinfo.RedirectStandardOutput = $true
            $pinfo.UseShellExecute = $false
            $pinfo.WorkingDirectory = ""{workingDir}""
            $p = New-Object System.Diagnostics.Process
            $p.StartInfo = $pinfo
            $p.Start() | Out-Null
            $p.WaitForExit()
            $output = $p.StandardOutput.ReadToEnd()
            $output += $p.StandardError.ReadToEnd()

            write-host ""{exitCodePrefix}$($p.ExitCode)""
            write-host ""{outputCodePrefix}`n$($output)""
        }}
        Invoke-Command -ComputerName $serverName -ScriptBlock $command
        ";
    }

    public string ExecutePsFileScript(string pathToScript)
    {
        _logger.Trace($"Выполняю скрипт в файле: '{pathToScript}'");

        var startInfo = new ProcessStartInfo
        {
            FileName = @"powershell.exe",
            Arguments = $@"& ""{pathToScript}""",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var process = new Process
        {
            StartInfo = startInfo
        };

        var isStarted = process.Start();
        process.WaitForExit();
        _logger.Debug($"Выполнил скрипт (IsStarted={isStarted}), код ошибки: {process.ExitCode}");

        var errors = process.StandardError.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(errors))
            return errors;

        var output = process.StandardOutput.ReadToEnd();
        return output;
    }
}

