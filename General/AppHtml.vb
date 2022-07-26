Imports System.IO
Imports System.Web.UI
Imports System.Web

Namespace AgoraNEO
    Public Class AppHtml
        Dim html As StringWriter
        Dim htmlWriter As HtmlTextWriter
        Dim newUrl As String

        '//Constructors 
        Sub New() ' MyHtmlFileCreator()
            html = New StringWriter
            htmlWriter = New HtmlTextWriter(html)
            newUrl = HttpContext.Current.Request.Url.AbsolutePath.ToString
            newUrl = newUrl.Replace(".aspx", ".htm")
        End Sub

        Function RenderHere() As HtmlTextWriter
            RenderHere = htmlWriter
        End Function

        Sub WriteHTMLFile(ByVal strVirtualFileName As String)
            Dim sr As New StringReader(html.ToString)
            Dim sw As New StringWriter
            Dim blnStart As Boolean = False
            Dim htmlLine As String
            htmlLine = sr.ReadLine

            Dim dDispatcher As New dispatcher
            sw.WriteLine("<LINK href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ rel=""stylesheet"">")


            While Not IsNothing(htmlLine)
                '' filter out asp.net specific tags
                'If blnContinue And blnStart Then

                'End If

                'If Not (htmlLine.IndexOf("<form") > 0 Or htmlLine.IndexOf("_VIEWSTATE") > 0 Or htmlLine.IndexOf("</form>") > 0) Then
                '    If htmlLine.IndexOf("<body") > 0 Then
                '        blnStart = True
                '        If Not blnContinue Then
                '            sw.WriteLine("<BODY MS_POSITIONING=""GridLayout"" BGCOLOR=""#ffffff"" onLoad=""window.focus();window.print();"">")
                '        End If
                '    Else
                '        If blnStart Then
                '            sw.WriteLine(htmlLine)
                '        End If
                '    End If
                'End If
                'htmlLine = sr.RAppCommonLine

                ' filter out asp.net specific tags
                If Not (htmlLine.IndexOf("<form") > 0 Or htmlLine.IndexOf("_VIEWSTATE") > 0 Or htmlLine.IndexOf("</form>") > 0) Then
                    sw.WriteLine(htmlLine)
                    ''If htmlLine.IndexOf("<body") > 0 Then
                    ''    'sw.WriteLine("<BODY BGCOLOR=""#ffffff"" onLoad=""parent.close();"">")
                    ''    'sw.WriteLine(htmlLine)
                    ''    'sw.WriteLine("<BODY MS_POSITIONING=""GridLayout"" BGCOLOR=""#ffffff"" onLoad=""window.focus();top.frames[0].location='FrameComplete.htm';"">")
                    ''Else
                    ''    sw.WriteLine(htmlLine)
                    ''End If
                End If
                htmlLine = sr.ReadLine

            End While

            Dim fs As StreamWriter = New StreamWriter(strVirtualFileName)
            fs.Write(sw.ToString)
            fs.Close()
        End Sub

        Sub WriteHTMLFile(ByVal strFileFrom As String, ByVal strFileTo As String)
            'Dim sr As New StringRAppCommoner(html.ToString)
            Dim sw As New StringWriter
            Dim blnStart As Boolean = False

            'Dim htmlLine As String
            'htmlLine = sr.RAppCommonLine

            Dim objStreamRAppCommoner1 As StreamReader
            Dim objStreamRAppCommoner2 As StreamReader
            objStreamRAppCommoner1 = File.OpenText(strFileFrom)
            objStreamRAppCommoner2 = File.OpenText(strFileTo)

            'Now, rAppCommon the entire file into a string
            Dim contents2 As String = objStreamRAppCommoner2.ReadLine
            Dim strAppName As String = HttpContext.Current.Request.ApplicationPath
            Dim strServerName As String = HttpContext.Current.Request.ServerVariables("HTTP_Host")
            '//
            'sw.WriteLine("<LINK href=""../css/Styles.css"" rel=""stylesheet"">")
            'sw.WriteLine("<LINK href=""" & HttpContext.Current.Server.MapPath("../css/Styles.css") & """ rel=""stylesheet"">")

            While Not IsNothing(contents2) ' src file
                ' copy all code from strfileto
                If Not (contents2.IndexOf("</body>") > 0 Or contents2.IndexOf("</HTML>") > 0) Then
                    sw.WriteLine(contents2)
                    If (contents2.IndexOf("</HAppCommon>") > 0) Then
                        sw.WriteLine("<STYLE>")
                        sw.WriteLine("	P.break { page-break-after: always }")
                        sw.WriteLine("</STYLE>")
                    End If
                End If
                contents2 = objStreamRAppCommoner2.ReadLine
            End While
            '"<P Class=""break"">&nbsp;</P>"
            sw.WriteLine("<P Class=""break"">&nbsp;</P>")

            Dim contents1 As String = objStreamRAppCommoner1.ReadLine
            While Not IsNothing(contents1) ' src file
                ' copy all code from strfileto
                If (contents1.IndexOf("<body") > 0) Then
                    blnStart = True
                End If
                If blnStart Then
                    sw.WriteLine(contents1)
                End If
                contents1 = objStreamRAppCommoner1.ReadLine
            End While
            'Dim fp As StreamRAppCommoner
            'Dim streamObj As Stream
            'streamObj = File.Open(strFileTo, FileMode.Append)
            'htmlLine = fp.RAppCommonLine
            'fp.Close()

            objStreamRAppCommoner1.Close()
            objStreamRAppCommoner2.Close()
            Dim fs As StreamWriter = New StreamWriter(strFileTo)
            fs.Write(sw.ToString)
            fs.Close()
        End Sub
    End Class
End Namespace

