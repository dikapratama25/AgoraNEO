Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Web
Imports System.Web.Mail
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Collections
Imports AgoraNEO
Imports System.Diagnostics

Namespace AgoraNEO
    Public Class AppDataProvider
        Public Function GetAllMasterCode(ByVal pCodeTableEnum As CodeTable, ByVal getStrSQL As Boolean) As Object
            Dim drw As DataView
            Dim lsSql As String
            Dim objDB As New EAD.DBCom

            Dim strCodeType As String
            strCodeType = System.Enum.GetName(GetType(CodeTable), pCodeTableEnum)
            If pCodeTableEnum = CodeTable.PaymentTerm Then
                lsSql = "SELECT CODE_ABBR,CODE_DESC FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' ORDER BY CODE_VALUE"
            Else
                lsSql = "SELECT CODE_ABBR,CODE_DESC FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' ORDER BY CODE_DESC"
            End If
            If getStrSQL Then
                Return lsSql
            Else
                Return objDB.GetView(lsSql)
            End If

            objDB = Nothing
        End Function

        Public Function GetMasterCodeByStatus(ByVal pCodeTableEnum As CodeTable, ByVal pStatus As String, ByVal getStrSQL As Boolean) As Object
            Dim drw As DataView
            Dim lsSql As String
            Dim objDB As New EAD.DBCom
            Dim strCodeType As String
            strCodeType = System.Enum.GetName(GetType(CodeTable), pCodeTableEnum)
            If pCodeTableEnum = CodeTable.PaymentTerm Or pCodeTableEnum = CodeTable.GRNCtrlDays Then
                lsSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' AND CODE_DELETED='" & pStatus & "' AND CODE_DESC != '' ORDER BY CONVERT(CODE_VALUE, SIGNED INTEGER)"
            Else
                lsSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' AND CODE_DELETED='" & pStatus & "' AND CODE_DESC != '' ORDER BY CODE_DESC"
            End If
            If getStrSQL Then
                Return lsSql
            Else
                Return objDB.GetView(lsSql)
            End If

            objDB = Nothing
        End Function

        'Public Function GetCommodityType(ByVal pCodeTableEnum As CodeTable, ByVal pStatus As String) As DataView
        '    Dim drw As DataView
        '    Dim lsSql As String
        '    Dim objDB As New EAD.DBCom
        '    Dim strCodeType As String
        '    strCodeType = System.Enum.GetName(GetType(CodeTable), pCodeTableEnum)
        '    If pCodeTableEnum = CodeTable.PaymentTerm Then
        '        lsSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' AND CODE_DELETED='" & pStatus & "' ORDER BY CODE_VALUE"
        '    Else
        '        lsSql = "SELECT CM.CODE_ABBR,CM.CODE_DESC,CC.CC_DEFAULT_CODE FROM CODE_MSTR CM,CODE_CATEGORY CC WHERE CM.CODE_CATEGORY=CC.CC_CODE AND CC.CC_DESC='" & strCodeType & "' AND CODE_DELETED='" & pStatus & "' ORDER BY CODE_DESC"
        '    End If
        '    drw = objDB.GetView(lsSql)
        '    objDB = Nothing
        '    Return drw
        'End Function

        Public Function LogToDB(ByVal pExp As Exception, ByVal pErrId As String, Optional ByVal pIn As String = "")
            Dim lsSql As String
            Dim objDB As New EAD.DBCom
            Dim ctx As System.Web.HttpContext = System.Web.HttpContext.Current
            Dim strOriginURL As String
            Dim strMessage As String
            '//**************************************************************
            '//Cannot Store Form Object into DB because it may contain huge amount of Data
            '//that come from ViewState
            '//QueryString may useful - still thinking
            'Dim strForm As String = ""
            'Dim strQueryString As String = ""
            'If Not ctx.Request.Form Is Nothing Then
            '    strForm = Convert.ToString(ctx.Request.Form)
            'End If
            'If Not ctx.Request.QueryString Is Nothing Then
            '    strQueryString = Convert.ToString(ctx.Request.QueryString)
            'End If
            '//***************************************************************
            If ctx.Request.ServerVariables("HTTP_REFERER") Is Nothing Then
                strOriginURL = "Cannot be detected"
            Else
                strOriginURL = ctx.Request.ServerVariables("HTTP_REFERER").ToString
            End If

            If pIn = "" Then
                strMessage = pExp.Message
            Else
                strMessage = "[" & pIn.ToString & "]--" & pExp.Message
            End If


            'lsSql = "INSERT INTO ERRORlOG_MSTR(Error_ID,Source,Message,StackTrace,Referer,Error_Date,Log_User,Log_Coy)Values('" & _
            'Common.Parse(pErrId) & "','" & _
            'Common.Parse(pExp.Source) & "','" & _
            'Common.Parse(strMessage).Substring(1, 2000) & "','" & _
            'Common.Parse(Trim(pExp.StackTrace)) & "','" & _
            'Common.Parse(strOriginURL).Substring(1, 500) & "',getdate(),'" & _
            'Common.Parse(ctx.Session("UserID")) & "','" & _
            'Common.Parse(ctx.Session("CompanyID")) & "')"
            lsSql = "INSERT INTO ERRORlOG_MSTR(Error_ID,Source,Message,StackTrace,Referer,Error_Date,Log_User,Log_Coy)Values('" &
            Common.Parse(pErrId) & "','" &
            Common.Parse(pExp.Source) & "','" &
            Common.Parse(strMessage) & "','" &
            Common.Parse(Trim(pExp.StackTrace)) & "','" &
            Common.Parse(strOriginURL) & "',now(),'" &
            Common.Parse(ctx.Session("UserID")) & "','" &
            Common.Parse(ctx.Session("CompanyID")) & "')"
            Try
                objDB.Execute(lsSql)
            Catch Err As Exception
                '//Cannot Call TrwExp
                '//Because LogToDB is called by TrwExp
                '//so calling TrwExp from here may result infinity loop or 
                '//actual exception not being captured properly
                'Wheel.Components.Common.TrwExp(Err, lsSql)
                EventLog.WriteEntry(Err.Source & "--" & lsSql, "Error Occured in Function LogToDB.", EventLogEntryType.Error, 65535)
            Finally
                objDB = Nothing
            End Try
        End Function

        Public Function getErrorLog() As DataView
            Dim lsSql As String
            Dim objDb As New EAD.DBCom
            lsSql = "Select * from ERRORLOG_MSTR"
            Return objDb.GetView(lsSql)
        End Function
    End Class

End Namespace
