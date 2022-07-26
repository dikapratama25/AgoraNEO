Public Class INIFile
    Private Declare Function WritePrivateProfileSection Lib "kernel32" Alias "WritePrivateProfileSectionA" (ByVal lpAppName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

    Public Function GetAppPath() As String
        Return System.IO.Path.GetDirectoryName( _
          System.Reflection.Assembly.GetExecutingAssembly().Location())
    End Function

    'Private Function FileExist(ByRef asPath As String) As Boolean
    '    Dim fso As Scripting.FileSystemObject
    '    Dim sCurrentProcess As String

    '    fso = New Scripting.FileSystemObject

    '    If fso.FileExists(asPath) Then
    '        FileExist = True
    '    Else
    '        FileExist = False
    '        sCurrentProcess = "File does not exist at '" & asPath & "'."
    '        MsgBox(sCurrentProcess, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "File Exist")
    '        appendToLog(Now, sCurrentProcess)
    '    End If

    'End Function

    Private Sub AppendToLog(ByRef dDate As Date, ByRef sProcedure As String, Optional ByRef sErrorNum As String = "", Optional ByRef sError As String = "")
        FileOpen(2, My.Application.Info.DirectoryPath & "\LogFile.txt", OpenMode.Append)
        PrintLine(2, Format(Now(), "dd/MMM/yyyy HH:mm:ss") & "  " & sProcedure & "  " & sErrorNum & " - " & sError)
        FileClose(2)
    End Sub

    Public Sub WriteIni(ByRef strFileName As String, ByRef strSecHeading As String, ByRef strVarName As String, ByRef strVarValue As String)
        WritePrivateProfileString(strSecHeading, strVarName, strVarValue, strFileName)
    End Sub

    Public Sub WriteIniSection(ByRef strFileName As String, ByRef strSecHeading As String, ByRef strVarValue As String)
        WritePrivateProfileSection(strSecHeading, strVarValue, strFileName)
    End Sub

    Public Function ReadIni(ByRef strFileName As String, ByRef strSecHeading As String, ByRef strVarName As String) As String

        Dim fnum As Short
        Dim one_line As String
        Dim pos As Short
        Dim strCurrSection As String = "", strTemp As String = ""

        fnum = FreeFile()
        FileOpen(fnum, strFileName, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)

        Do While Not EOF(fnum)
            ' Read a line.
            one_line = LineInput(fnum)

            ' See what it is.
            If Left(one_line, 1) = "[" Then
                ' Section heading.
                strCurrSection = Mid(one_line, 2, Len(one_line) - 2)
            ElseIf InStr(one_line, "=") > 0 Then
                If strCurrSection = strSecHeading Then
                    pos = InStr(one_line, "=")
                    If Left(one_line, pos - 1) = strVarName Then
                        strTemp = Mid(one_line, pos + 1)
                    End If
                End If
            Else

            End If
        Loop

        FileClose(fnum)
        ReadIni = strTemp
    End Function

    'Public Function GetIniFileValue(ByRef strFileName As String, ByRef strSecHeading As String, ByRef strVarName As String) As String
    '    Dim INIValue, sCurrentProcess As String
    '    If FileExist(strFileName) Then
    '        INIValue = ReadIni(strFileName, strSecHeading, strVarName)
    '        If Not IsNothing(INIValue) Then
    '            GetIniFileValue = INIValue
    '        Else
    '            sCurrentProcess = "Unable to read INI file. File might be corrupted."
    '            MsgBox(sCurrentProcess, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Read INI Files")
    '            AppendToLog(Now, sCurrentProcess)
    '            GetIniFileValue = ""
    '            Exit Function
    '        End If
    '    Else
    '        GetIniFileValue = ""
    '    End If

    'End Function
End Class
