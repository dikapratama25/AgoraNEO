Imports System.Web.UI.Page
Imports System.IO
Imports System.Web
Imports System.Configuration

'Imports AgoraNEO
'Imports AgoraNEO
'Imports System.Data
'Imports Microsoft.Reporting.WebForms
'Imports System.IO
'Imports MySql.Data.MySqlClient

'Public Class PreviewPO1FTN
'Inherits AppBaseClass

Namespace AgoraNEO
    Public Class dispatcher
        Dim sXML As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "Bin\SPP.xml"

        Private Function GetWhiteLabel(ByVal sFunction As String, ByVal sEnv As String) As ArrayList
            Dim XMLReader As Xml.XmlReader, sRet As New ArrayList
            XMLReader = New Xml.XmlTextReader(sXML)

            While XMLReader.Read
                Select Case XMLReader.NodeType
                    Case Xml.XmlNodeType.Element
                        If XMLReader.AttributeCount > 0 Then
                            While XMLReader.MoveToNextAttribute
                                If XMLReader.Value = sEnv Then
                                    XMLReader.MoveToNextAttribute()
                                    If XMLReader.Name = "path" Then sRet.Add(XMLReader.Value)

                                    While XMLReader.Read
                                        While XMLReader.MoveToNextAttribute
                                            If XMLReader.Name = "type" And XMLReader.Value = "wl" Then
                                                XMLReader.Close()
                                            End If
                                            If LCase(sFunction) = LCase(XMLReader.Value) Then
                                                sRet.Add(XMLReader.ReadString)
                                                XMLReader.Close()
                                            End If
                                        End While
                                    End While
                                End If
                            End While
                        End If
                    Case Xml.XmlNodeType.Text
                    'Debug.WriteLine(XMLReader.Value)
                    Case Xml.XmlNodeType.Comment
                        'Debug.WriteLine(XMLReader.Value)
                End Select
            End While
            XMLReader.Close()

            GetWhiteLabel = sRet
        End Function

        Private Function GetCommonPath() As ArrayList
            Dim XMLReader As Xml.XmlReader, sRet As New ArrayList
            XMLReader = New Xml.XmlTextReader(sXML)
            Dim sEnv As String = "Common"

            While XMLReader.Read
                Select Case XMLReader.NodeType
                    Case Xml.XmlNodeType.Element
                        If XMLReader.AttributeCount > 0 Then
                            While XMLReader.MoveToNextAttribute
                                If XMLReader.Value = sEnv Then
                                    XMLReader.MoveToNextAttribute()
                                    If XMLReader.Name = "path" Then
                                        sRet.Add(XMLReader.Value)
                                        XMLReader.Close()
                                    End If
                                    'While XMLReader.Read
                                    '    While XMLReader.MoveToNextAttribute
                                    '        If sFunction = XMLReader.Value Then
                                    '            sRet.Add(XMLReader.ReadString)
                                    '            XMLReader.Close()
                                    '        End If
                                    '    End While
                                    'End While
                                End If

                            End While
                        End If
                    Case Xml.XmlNodeType.Text
                    'Debug.WriteLine(XMLReader.Value)
                    Case Xml.XmlNodeType.Comment
                        'Debug.WriteLine(XMLReader.Value)
                End Select
            End While
            XMLReader.Close()


            GetCommonPath = sRet

        End Function

        Private Function getPath(ByVal sPath As String, ByVal sFolder As String, ByVal sFileName As String, Optional ByVal sGet As String = "") As String
            Dim sRet As String
            sRet = sPath & sFolder & "/" & sFileName
            If Len(sGet) > 0 Then sRet = sRet & "?" & sGet
            getPath = sRet
        End Function

        Private Function getPath2(ByVal sFolder As String, ByVal sLang As String, ByVal sFileName As String) As String
            Dim sRet As String

            If sLang = "zh" Then
                sRet = "../" & sFolder & "/" & sLang & "/" & sFileName 'Zh Folder
            Else
                sRet = "../" & sFolder & "/en/" & sFileName 'En Folder
            End If

            getPath2 = sRet
        End Function

        Public Function direct(ByVal sFolder As String, ByVal sFileName As String, Optional ByVal sGet As String = "") As String
            Dim sEnv, sB1Env, sB2Env, sB3Env, sB4Env, sB5Env, sGetR As String
            Dim aryDirector As New ArrayList
            Dim appPath As String = System.AppDomain.CurrentDomain.BaseDirectory

            sEnv = ConfigurationManager.AppSettings.Get("Env")
            sB1Env = ConfigurationManager.AppSettings.Get("BaseEnv1")
            sB2Env = ConfigurationManager.AppSettings.Get("BaseEnv2")
            sB3Env = ConfigurationManager.AppSettings.Get("BaseEnv3")
            sB4Env = ConfigurationManager.AppSettings.Get("BaseEnv4")
            sB5Env = ConfigurationManager.AppSettings.Get("BaseEnv5")

            If sGet = "Report" Or sGet = "FTNReport" Then
                sGetR = ""
            Else
                sGetR = sGet
            End If

            'If sGet = "Report" Then
            '    Select Case sEnv
            '        '' ''Case "Common"
            '        '' ''    direct = ConfigurationManager.AppSettings.Get("CommonReportPath") & sFolder & "\" & sFileName
            '        Case "FTN"
            '            direct = ConfigurationManager.AppSettings.Get("" & sEnv & "ReportPath1") & sFolder & "\" & sFileName
            '            '' ''Case "SEH"
            '            '' ''    direct = ConfigurationManager.AppSettings.Get("SEHReportPath") & sFolder & "\" & sFileName
            '        Case Else
            '            direct = ConfigurationManager.AppSettings.Get("" & sEnv & "ReportPath") & sFolder & "\" & sFileName
            '    End Select

            'ElseIf sGet = "FTNReport" Then
            '    Select Case sEnv
            '        'Case "Common"
            '        '    direct = ConfigurationManager.AppSettings.Get("CommonReportPath") & sFolder & "\" & sFileName
            '        Case "FTN"
            '            direct = ConfigurationManager.AppSettings.Get("" & sEnv & "ReportPath") & sFolder & "\" & sFileName
            '            'Case "SEH"
            '            '    direct = ConfigurationManager.AppSettings.Get("SEHReportPath") & sFolder & "\" & sFileName

            '    End Select

            'Else
            '    If InStr(sFileName, ".") > 0 Then
            '        If sEnv = "Common" Then
            '            aryDirector = GetCommonPath()
            '            direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '        Else
            '            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sEnv)
            '            If aryDirector.Count < 2 Then
            '                If sB1Env <> "" Then
            '                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB1Env)
            '                    If aryDirector.Count < 2 Then
            '                        If sB2Env <> "" Then
            '                            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB2Env)
            '                            If aryDirector.Count < 2 Then
            '                                If sB3Env <> "" Then
            '                                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB3Env)
            '                                    If aryDirector.Count < 2 Then
            '                                        If sB4Env <> "" Then
            '                                            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB4Env)
            '                                            If aryDirector.Count < 2 Then
            '                                                If sB5Env <> "" Then
            '                                                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB5Env)
            '                                                    If aryDirector.Count < 2 Then
            '                                                        aryDirector = GetCommonPath()
            '                                                        direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                                    Else
            '                                                        direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                                    End If
            '                                                Else
            '                                                    aryDirector = GetCommonPath()
            '                                                    direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                                    Exit Function
            '                                                End If
            '                                            Else
            '                                                direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                            End If
            '                                        Else
            '                                            aryDirector = GetCommonPath()
            '                                            direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                            Exit Function
            '                                        End If
            '                                    Else
            '                                        direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                    End If
            '                                Else
            '                                    aryDirector = GetCommonPath()
            '                                    direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                                    Exit Function
            '                                End If
            '                            Else
            '                                direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                            End If
            '                        Else
            '                            aryDirector = GetCommonPath()
            '                            direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                            Exit Function
            '                        End If
            '                    Else
            '                        direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                    End If
            '                Else
            '                    aryDirector = GetCommonPath()
            '                    direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '                    Exit Function
            '                End If

            '            Else
            '                direct = getPath(aryDirector(0), sFolder, sFileName, sGet)
            '            End If
            '        End If
            '    Else
            '        'Not valid file extension
            '        direct = "#"
            '    End If

            'End If

            If InStr(sFileName, ".") > 0 Then
                If sEnv = "Common" Then
                    aryDirector = GetCommonPath()
                    direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                Else
                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sEnv)
                    If aryDirector.Count < 2 Then
                        If sB1Env <> "" Then
                            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB1Env)
                            If aryDirector.Count < 2 Then
                                If sB2Env <> "" Then
                                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB2Env)
                                    If aryDirector.Count < 2 Then
                                        If sB3Env <> "" Then
                                            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB3Env)
                                            If aryDirector.Count < 2 Then
                                                If sB4Env <> "" Then
                                                    aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB4Env)
                                                    If aryDirector.Count < 2 Then
                                                        If sB5Env <> "" Then
                                                            aryDirector = GetWhiteLabel(Mid(sFileName, 1, InStr(sFileName, ".") - 1), sB5Env)
                                                            If aryDirector.Count < 2 Then
                                                                aryDirector = GetCommonPath()
                                                                direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                                            Else
                                                                direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                                            End If
                                                        Else
                                                            aryDirector = GetCommonPath()
                                                            direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                                            GoTo Replace
                                                        End If
                                                    Else
                                                        direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                                    End If
                                                Else
                                                    aryDirector = GetCommonPath()
                                                    direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                                    GoTo Replace
                                                End If
                                            Else
                                                direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                            End If
                                        Else
                                            aryDirector = GetCommonPath()
                                            direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                            GoTo Replace
                                        End If
                                    Else
                                        direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                    End If
                                Else
                                    aryDirector = GetCommonPath()
                                    direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                                    GoTo Replace
                                End If
                            Else
                                direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                            End If
                        Else
                            aryDirector = GetCommonPath()
                            direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                            GoTo Replace
                        End If

                    Else
                        direct = getPath(aryDirector(0), sFolder, sFileName, sGetR)
                    End If
                End If
            Else
                'Not valid file extension
                direct = "#"
            End If
Replace:
            If sGet = "Report" Or sGet = "FTNReport" Then
                direct = direct.Replace("../../", appPath)
                direct = direct.Replace("/", "\")

                If direct = "#" Then
                    aryDirector = GetCommonPath()
                    direct = aryDirector(0).Replace("../../", appPath) & "Report\Temp"
                    'direct = direct.Replace("Common", sEnv)
                    direct = direct.Replace("/", "\")

                    If sFileName = "" Then
                        direct = aryDirector(0).Replace("../../", appPath) & "Plugins\images\"
                        'direct = direct.Replace("Common", sEnv)
                        direct = direct.Replace("/", "\")
                    End If
                End If
            End If

            'If aryDirector.Count = 2 Then
            '    direct = aryDirector(0) & aryDirector(1)
            '    If Len(sPara) > 0 Then direct = direct & "?" & sPara
            'Else
            '    direct = "#"
            'End If
            'System.Web.HttpContext.Current.Response
            'System.Web.HttpContext.Current.Request.PhysicalApplicationPath & aryDirector(1)
        End Function

        Public Function splitter(ByVal sFullName As String) As ArrayList
            Dim sRet As New ArrayList

            Dim sFullFolder() As String
            Dim sFolder As String = ""
            sFullFolder = Split(sFullName, "/")
            For iFolder As Integer = 0 To UBound(sFullFolder) - 1
                sFolder = sFolder & sFullFolder(iFolder) & "/"
            Next iFolder

            If InStr(sFolder, "/") > 0 Then
                If Mid(sFolder, Len(sFolder)) = "/" Then
                    sRet.Add(Mid(sFolder, 1, Len(sFolder) - 1))
                Else
                    sRet.Add(sFolder)
                End If
            Else
                sRet.Add(sFolder)
            End If

            Dim sRem = sFullFolder(sFullFolder.Length - 1)
            If InStr(sRem, "?") > 0 Then
                sRet.Add(Mid(sRem, 1, InStr(sRem, "?") - 1))
                sRet.Add(Mid(sRem, InStr(sRem, "?") + 1))
            Else
                sRet.Add(sRem)
                sRet.Add("")
            End If

            splitter = sRet
        End Function

        Public Function getTemplate(ByVal sFolder As String, ByVal sFileName As String) As String

            Dim lang As String
            Dim ParamLanguage As String = ""
            Dim arrLang As Array = HttpContext.Current.Request.UserLanguages

            If Not IsDBNull(arrLang) Or arrLang.Length > 0 Then
                ParamLanguage = arrLang(0).ToString()
            End If
            lang = Mid(ParamLanguage.ToLower, 1, 2)

            Dim sEnv As String, aryDirector As New ArrayList
            sEnv = ConfigurationManager.AppSettings.Get("Env")

            If InStr(sFileName, ".") > 0 Then
                aryDirector = GetWhiteLabel(Mid(sFolder, 1, InStr(sFileName, ".") - 1), sEnv)
                If aryDirector.Count < 2 Then
                    aryDirector = GetCommonPath()
                    getTemplate = getPath2(sFolder, lang, sFileName)

                Else
                    getTemplate = getPath2(sFolder, lang, sFileName)
                End If
            Else
                getTemplate = "#"
            End If

        End Function
        'Private Function GetFromNonCommon(ByVal sFunction As String, ByVal sEnv As String) As ArrayList
        '    Dim XMLReader As Xml.XmlReader, sRet As New ArrayList
        '    XMLReader = New Xml.XmlTextReader(sXML)

        '    While XMLReader.Read
        '        Select Case XMLReader.NodeType
        '            Case Xml.XmlNodeType.Element
        '                If XMLReader.AttributeCount > 0 Then
        '                    While XMLReader.MoveToNextAttribute
        '                        If XMLReader.Value = sEnv Then
        '                            XMLReader.MoveToNextAttribute()
        '                            If XMLReader.Name = "path" Then sRet.Add(XMLReader.Value)

        '                            While XMLReader.Read
        '                                While XMLReader.MoveToNextAttribute
        '                                    If XMLReader.Name = "type" And XMLReader.Value = "wl" Then
        '                                        XMLReader.Close()
        '                                    End If
        '                                    If sFunction = XMLReader.Value Then
        '                                        sRet.Add(XMLReader.ReadString)
        '                                        XMLReader.Close()

        '                                    End If
        '                                End While
        '                            End While
        '                        End If

        '                    End While
        '                End If
        '            Case Xml.XmlNodeType.Text
        '                'Debug.WriteLine(XMLReader.Value)
        '            Case Xml.XmlNodeType.Comment
        '                'Debug.WriteLine(XMLReader.Value)
        '        End Select
        '    End While
        '    XMLReader.Close()

        '    GetFromNonCommon = sRet
        'End Function
        'Private Function GetFromCommon(ByVal sFile As String) As ArrayList
        '    Dim XMLReader As Xml.XmlReader, sRet As New ArrayList
        '    XMLReader = New Xml.XmlTextReader(sXML)
        '    Dim sEnv As String = "common"

        '    While XMLReader.Read
        '        Select Case XMLReader.NodeType
        '            Case Xml.XmlNodeType.Element
        '                If XMLReader.AttributeCount > 0 Then
        '                    While XMLReader.MoveToNextAttribute
        '                        If XMLReader.Value = sEnv Then
        '                            XMLReader.MoveToNextAttribute()
        '                            If XMLReader.Name = "path" Then sRet.Add(XMLReader.Value)

        '                            While XMLReader.Read
        '                                While XMLReader.MoveToNextAttribute
        '                                    If sFile = XMLReader.Value Then
        '                                        sRet.Add(XMLReader.ReadString)
        '                                        XMLReader.Close()
        '                                    End If
        '                                End While
        '                            End While
        '                        End If

        '                    End While
        '                End If
        '            Case Xml.XmlNodeType.Text
        '                'Debug.WriteLine(XMLReader.Value)
        '            Case Xml.XmlNodeType.Comment
        '                'Debug.WriteLine(XMLReader.Value)
        '        End Select
        '    End While
        '    XMLReader.Close()


        '    GetFromCommon = sRet

        'End Function

        'Private Sub btnFTN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFTN.Click
        '    Dim aryNonCommon As New ArrayList, aryCommon As New ArrayList
        '    aryNonCommon = GetFromNonCommon("addDO", "FTN")
        '    If aryNonCommon.Count > 1 Then
        '        MsgBox("Get from FTN")
        '    Else
        '        aryCommon = GetFromCommon("addDO")
        '        MsgBox("Get from common")
        '    End If
        '    System.Web.HttpContext.Current.Request.ServerVariables("SERVER_NAME")

        'End Sub
        'Private Sub btnCommon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommon.Click
        '    Dim aryCommon As New ArrayList
        '    aryCommon = GetFromCommon("addDO") 'editDO
        '    MsgBox("Get from common")
        'End Sub
    End Class
End Namespace
