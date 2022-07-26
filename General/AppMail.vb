Imports System
Imports System.Web.HttpContext
'Imports System.Web.Mail
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web.HttpServerUtility
'Imports EASendMail 
Imports System.Net.Mail 'mimi 14-03-2018

Namespace AgoraNEO
    Public Class AppMail
        Public MailTo As String
        Public MailFrom As String
        Public MailAcct As String
        Public MailAcctP As String
        Public MailAcct2 As String
        Public MailAcctP2 As String
        Public MailAcct3 As String
        Public MailAcctP3 As String
        Public Subject As String
        Public Body As String
        Public MailCc As String
        Public MailBCc As String
        Public MailServer As String
        Public MailPort As Integer
        Public Attachment As ArrayList
        Public Priority As Integer

        '//Constructors 
        Public Sub New()
            ' initialization
            Priority = MailPriority.Normal
            MailServer = ConfigurationSettings.AppSettings("MailServer")
            MailPort = ConfigurationSettings.AppSettings("MailServerPort")
            MailFrom = ConfigurationSettings.AppSettings("HubAdminEmail")
            MailAcct = ConfigurationSettings.AppSettings("MailServerAcct")
            MailAcctP = ConfigurationSettings.AppSettings("MailServerAcctP")
            MailAcct2 = ConfigurationSettings.AppSettings("MailServerAcct2")
            MailAcctP2 = ConfigurationSettings.AppSettings("MailServerAcctP2")
            MailAcct3 = ConfigurationSettings.AppSettings("MailServerAcct3")
            MailAcctP3 = ConfigurationSettings.AppSettings("MailServerAcctP3")
            MailCc = ConfigurationSettings.AppSettings("EmailccTo")
        End Sub

        'mimi 14-03-2018
        Public Sub SendMail()
            Dim msg As New MailMessage()

            ' Your mail address and display name.
            ' This what will appear on the From field.
            ' If you used another credentials to access
            ' the SMTP server, the mail message would be
            ' sent from the mail specified in the From
            ' field on behalf of the real sender.
            Try
                msg.From = _
                    New MailAddress(MailAcct)

                'Jules 2018.07.18 - ";" causing 'System.FormatException: The specified string is not in the form required for an e-mail address.'
                ' To addresses
                If Not MailTo Is Nothing Then 'AndAlso Regex.IsMatch(MailTo, strMailPattern) Then
                    msg.To.Add(Replace(MailTo, ";", ","))
                End If

                If Not MailCc Is Nothing Then
                    msg.CC.Add(Replace(MailCc, ";", ","))
                End If

                If Not MailBCc Is Nothing Then
                    msg.Bcc.Add(Replace(MailBCc, ";", ","))
                End If

                ' You can specify CC and BCC addresses also

                ' Set to high priority
                msg.Priority = Priority

                msg.Subject = Subject

                ' You can specify a plain text or HTML contents
                msg.Body = Body
                ' In order for the mail client to interpret message
                ' body correctly, we mark the body as HTML
                ' because we set the body to HTML contents.
                msg.IsBodyHtml = True

                'Attaching some data
                If Not Attachment Is Nothing Then
                    If Attachment.Count > 0 Then
                        Dim sFilePath As String
                        For intCnt As Integer = 0 To Attachment.Count - 1
                            'Dim PostedFile As System.Web.HttpPostedFile
                            'PostedFile = Attachment(intCnt)
                            'If Not PostedFile Is Nothing And PostedFile.ContentLength > 0 Then
                            '    Dim strFileName As String = Path.GetFileName(PostedFile.FileName)
                            '    msg.Attachments.Add(New Attachment(System.Web.HttpContext.Current.Server.MapPath("AttachFile") & "\" & strFileName))
                            'End If
                        Next
                    End If
                End If

                ' Connecting to the server and configuring it
                Dim client As New SmtpClient()
                client.Host = MailServer
                client.Port = MailPort
                client.EnableSsl = False
                ' The server requires user's credentials
                ' not the default credentials
                client.UseDefaultCredentials = True
                ' Provide your credentials
                client.Credentials = New System.Net.NetworkCredential(MailAcct, MailAcctP)
                client.DeliveryMethod = SmtpDeliveryMethod.Network

                Try
                    ' Use SendAsync to send the message asynchronously
                    client.Send(msg)
                    Exit Try
                Catch ex1 As Exception
                    'Console.WriteLine(ex1.Message)
                    If ex1.Message.Contains("5.4.5") Then
                        client.Credentials = New System.Net.NetworkCredential(MailAcct2, MailAcctP2)
                        Try
                            ' Use SendAsync to send the message asynchronously
                            client.Send(msg)
                            Exit Try
                        Catch ex2 As Exception
                            'Console.WriteLine(ex2.Message)

                            If ex2.Message.Contains("5.4.5") Then
                                client.Credentials = New System.Net.NetworkCredential(MailAcct3, MailAcctP3)
                                client.Send(msg)
                            End If
                            Exit Try
                        End Try
                    End If
                    Exit Try
                End Try
            Catch ep As Exception
                Common.WriteLog(Now & vbTab & "SendMail : " & ep.ToString)
                While Not (ep.InnerException Is Nothing)
                    Common.WriteLog(Now & vbTab & "SendMail : " & ep.InnerException.ToString())
                    ep = ep.InnerException
                End While
            End Try
            'Console.ReadLine()
        End Sub
#Region "SendMail EASEndMail"
        '' ''Public Sub SendMail_EASendMail()
        '' ''    Dim oMail As New EASendMail.SmtpMail("TryIt")
        '' ''    Dim oSmtp As New SmtpClient()

        '' ''    'Gmail SMTP server address
        '' ''    Dim oServer As New SmtpServer(MailServer)

        '' ''    ' set 587 port, if you want to use 25 port, please change 587 to 25
        '' ''    oServer.Port = MailPort

        '' ''    ' detect SSL/TLS automatically
        '' ''    oServer.ConnectType = SmtpConnectType.ConnectSSLAuto
        '' ''    ' Gmail user authentication should use your
        '' ''    ' Gmail email address as the user name.

        '' ''    oServer.User = MailAcct
        '' ''    oServer.Password = MailAcctP

        '' ''    Try
        '' ''        If Not MailTo Is Nothing Then 'AndAlso Regex.IsMatch(MailTo, strMailPattern) Then
        '' ''            oMail.To = MailTo
        '' ''        End If

        '' ''        If Not MailCc Is Nothing Then
        '' ''            oMail.Cc = MailCc
        '' ''        End If

        '' ''        If Not MailBCc Is Nothing Then
        '' ''            oMail.Bcc = MailBCc
        '' ''        End If

        '' ''        'If Regex.IsMatch(MailFrom, strMailPattern) Then
        '' ''        oMail.From = MailFrom

        '' ''        'Set the priority - options are High, Low, and Normal
        '' ''        oMail.Priority = Priority

        '' ''        'Set the subject
        '' ''        oMail.Subject = Subject

        '' ''        'Set the body
        '' ''        oMail.HtmlBody = "<html><body>" & Body & "</body></html>"
        '' ''        'objMM.Body = Body

        '' ''        If Not Attachment Is Nothing Then
        '' ''            If Attachment.Count > 0 Then
        '' ''                Dim sFilePath As String
        '' ''                For intCnt As Integer = 0 To Attachment.Count - 1
        '' ''                    Dim PostedFile As System.Web.HttpPostedFile
        '' ''                    PostedFile = Attachment(intCnt)
        '' ''                    If Not PostedFile Is Nothing And PostedFile.ContentLength > 0 Then
        '' ''                        Dim strFileName As String = Path.GetFileName(PostedFile.FileName)
        '' ''                        oMail.AddAttachment(System.Web.HttpContext.Current.Server.MapPath("AttachFile") & "\" & strFileName)
        '' ''                    End If
        '' ''                Next
        '' ''            End If
        '' ''        End If

        '' ''        oSmtp.SendMail(oServer, oMail)

        '' ''    Catch ep As Exception
        '' ''        Common.WriteLog(Now & vbTab & "SendMail : " & ep.ToString)
        '' ''        While Not (ep.InnerException Is Nothing)
        '' ''            Common.WriteLog(Now & vbTab & "SendMail : " & ep.InnerException.ToString())
        '' ''            ep = ep.InnerException
        '' ''        End While
        '' ''    End Try
        '' ''End Sub
#End Region

        Sub getEmailAddr(ByVal strUserID As String, ByVal strCoyId As String, ByVal strCoyType As String)
            Dim strSql As String
            If strCoyType = "V" Then '//Vendor
                strSql = "SELECT CM_EMAIL FROM COMPANY_MSTR WHERE CM_COY_ID='" & strCoyId & "'"
            Else
                strSql = "SELECT UM_EMAIL,UM_USER_NAME,UM_STATUS FROM USER_MSTR WHERE UM_USER_ID='" & strUserID & "' AND UM_COY_ID='" & strCoyId & "'"
            End If
        End Sub

        ''        Public Function sendNotification(ByVal pEmailType As EmailType, ByVal strUserId As String, _
        ''                                        ByVal strBCoyId As String, ByVal strSCoyId As String, _
        ''                                        ByVal strDocNo As String, ByVal strDocNo2 As String, _
        ''                                        Optional ByVal pPara1 As String = "", _
        ''                                        Optional ByVal pPara2 As String = "")
        ''            Dim strsql As String
        ''            Dim ds As New DataSet
        ''            Dim strBody, strCC, strTo, strSubject As String
        ''            Dim objUser As New User
        ''            Dim objUsers As New Users
        ''            Dim i As Integer
        ''            Dim blnAttn As Boolean = False
        ''            Dim objComp As New Company
        ''            Dim objComps As New Companies
        ''            Dim strCond As String
        ''            Dim strExtra As String = ""
        ''            Dim strLine As String
        ''            Dim strEmailType As String
        ''            ' strEmailType - 1 = active user exists
        ''            '                2 = deactive user exists
        ''            '                3 = no user exists
        ''            '                4 = company not active

        ''            strTo = ""

        ''            Dim dtAttn As New DataTable
        ''            dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
        ''            dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
        ''            dtAttn.Columns.Add("Email", Type.GetType("System.String"))
        ''            dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
        ''            Dim dtr As DataRow

        ''            Select Case pEmailType
        ''                Case EmailType.AckGRN ' SK to 2nd 
        ''                    strSubject = "GRN Acknowledgement"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    strCond = "AND UM_USER_ID IN (SELECT UL_USER_ID FROM GRN_MSTR LEFT JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX "
        ''                    strCond &= "LEFT JOIN USERS_LOCATION ON UL_ADDR_CODE = DOM_D_ADDR_CODE AND UL_COY_ID = GM_B_COY_ID "
        ''                    strCond &= "WHERE UL_LEVEL = 2 AND GM_GRN_NO = '" & strDocNo & "' AND GM_B_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Second_Level_Receiver, strBCoyId, strCond)

        ''                    strBody = "Dear 2nd Level Storekeeper, <BR>"
        ''                    strBody &= "<P>You have received a new GRN (" & strDocNo & ") from " & objUser.Name & ". <BR>"
        ''                    strBody &= "<P>Please login to " & EmailHomeEhubAddr & " to acknowledge it. <BR>"

        ''                Case EmailType.DOCreated ' Vendor to Buyer
        ''                    strSubject = "DO Created"
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor
        ''                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo2 & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>DO (" & strDocNo & ") has been created for PO (" & strDocNo2 & ") by " & objComp.CoyName & ". <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.DOCreatedToSK ' Vendor to SK
        ''                    strSubject = "DO Created"
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor
        ''                    strCond = "AND UM_USER_ID IN (SELECT UL_USER_ID FROM DO_MSTR "
        ''                    strCond &= "LEFT JOIN USERS_LOCATION ON UL_ADDR_CODE = DOM_D_ADDR_CODE AND UL_COY_ID = '" & strBCoyId & "' "
        ''                    strCond &= "WHERE UL_LEVEL = 1 AND DOM_DO_NO = '" & strDocNo & "' )"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

        ''                    strBody = "Dear Storekeeper, <BR>"
        ''                    strBody &= "<P>DO (" & strDocNo & ") has been created for PO (" & strDocNo2 & ") by " & objComp.CoyName & ". <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.GoodsReceiptNoteCreated 'Buyer to Vendor
        ''                    ' pPara1 = GRN No
        ''                    strSubject = "GRN Created"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId) ' buyer 
        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

        ''                    strBody = "Dear Vendor, <BR>"
        ''                    strBody &= "<P>Goods from DO (" & strDocNo & ") for PO (" & strDocNo2 & ") have been received and accepted by the buyer. <BR>"
        ''                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has created a GRN (" & pPara1 & ") "
        ''                    strBody &= "for PO (" & strDocNo2 & ") on " & FormatDateTime(Today, Microsoft.VisualBasic.DateFormat.ShortDate) & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.GoodsReceiptNoteReject 'Buyer to Vendor
        ''                    ' pPara1 = GRN No
        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)
        ''                    strSubject = "GRN Created With Rejection"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)

        ''                    strBody = "Dear Vendor, <BR>"
        ''                    strBody &= "<P>Goods from your DO (" & strDocNo & ") for PO (" & strDocNo2 & ") have been rejected. <BR>"
        ''                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has created a GRN (" & pPara1 & ") "
        ''                    strBody &= "with rejection for PO (" & strDocNo2 & ") <BR>on " & FormatDateTime(Today, Microsoft.VisualBasic.DateFormat.ShortDate) & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to  " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.InvoiceApproval ' Buyer - Finance Officer to Finance Manager
        ''                    strSubject = "Invoice Approval"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId)

        ''                    strBody = "Dear Finance Manager, <BR>"
        ''                    strBody &= "<P>You have an invoice (" & strDocNo & ") awaiting your approval. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.InvoiceCreated ' Vendor to Buyer(Finance Officer)
        ''                    strSubject = "Invoice Created"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Finance_Officer, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Finance Officer, <BR>"
        ''                    strBody &= "<P>You have received an invoice (" & strDocNo & ") created by vendor " & objComp.CoyName & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.POAccepted ' Vendor to Buyer
        ''                    strSubject = "PO Accepted"
        ''                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Buyer/Consolidator, <BR>"
        ''                    strBody &= "<P>Your PO (" & strDocNo & ") has been accepted by " & objComp.CoyName & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.POCancellationRequest ' Buyer to Vendor
        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)
        ''                    strSubject = "PO Cancellation Request"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)

        ''                    strBody = "Dear Vendor, <BR>"
        ''                    strBody &= "<P>Buyer " & objUser.Name & " of " & objComp.CoyName & " has requested to cancel PO (" & strDocNo & "). <BR>"
        ''                    strBody &= "<P>Please login to " & EmailHomeEhubAddr & " to respond to the request. <BR>"

        ''                Case EmailType.AckPOCancellationRequest ' Vendor to Buyer
        ''                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
        ''                    strSubject = "PO Cancellation Request - Acknowledgement"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strSCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Buyer/Consolidator, <BR>"
        ''                    strBody &= "<P>Your cancellation request for PO (" & strDocNo & ") has been acknowledged by " & objUser.Name & " from " & objComp.CoyName & ". <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.POCreated ' to buyer
        ''                    strSubject = "PO Created"
        ''                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>Your PR (" & strDocNo & ") has been converted into a PO (" & strDocNo2 & ") and sent to " & objComp.CoyName & " company. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.PORaised ' Buyer to Vendor
        ''                    strSubject = "PO Raised"
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)
        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

        ''                    strBody = "Dear Vendor, <BR>"
        ''                    strBody &= "<P>You have received a new PO (" & strDocNo & ") from " & objComp.CoyName & " Buyer. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.PORejected ' Vendor to Buyer (PO owner)
        ''                    strSubject = "PO Rejected"
        ''                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Approving Officer, <BR>"
        ''                    strBody &= "<P>Your PO (" & strDocNo & ") has been rejected by " & objComp.CoyName & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.PRConsolidated
        ''                    strSubject = "PR Consolidation"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    strCond = "AND UM_USER_ID IN (SELECT PRM_CONSOLIDATOR FROM PR_MSTR "
        ''                    strCond &= "WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Consolidator, strBCoyId, strCond)

        ''                    strBody = "Dear Consolidator, <BR>"
        ''                    strBody &= "<P>You have a PR (" & strDocNo & ") waiting for consolidation. <BR>"
        ''                    strBody &= "<P>Please go to " & EmailHomeAddr & " to view it. <BR>"

        ''                Case EmailType.PRCancelled ' Buyer to AO
        ''                    strSubject = "PR Cancelled"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    strCond = "AND UM_USER_ID IN (SELECT TOP 1 PRA_AO FROM PR_MSTR LEFT JOIN PR_APPROVAL ON PRM_PR_INDEX = PRA_PR_INDEX "
        ''                    strCond &= "WHERE PRM_PR_NO = '" & strDocNo & "' "
        ''                    strCond &= "AND PRM_COY_ID = '" & strBCoyId & "' "
        ''                    strCond &= "ORDER BY PRA_SEQ)"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)

        ''                    strBody = "Dear Approving Officer, <BR>"
        ''                    strBody &= "<P>PR (" & strDocNo & ") has been cancelled by " & objUser.Name & ". <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.PRHeld ' AO to Buyer
        ''                    strSubject = "PR Held"
        ''                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>Your Approving Officer has put your PR (" & strDocNo & ") on hold. <BR>"
        ''                    strBody &= "<P>For more details, please login to  " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.PRRejected ' AO to Buyer
        ''                    strSubject = "PR Rejected"
        ''                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>Your PR (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.RFQRequested ' Buyer to Vendor
        ''                    ' strdocNo2 = expired date
        ''                    strSubject = "RFQ Requested"
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

        ''                    strBody = "Dear Vendor, <BR>"
        ''                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has requested a RFQ (" & strDocNo & ") which will expired on " & strDocNo2 & ". <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.RFQRejected ' vendor to buyer
        ''                    strSubject = "RFQ Rejected"
        ''                    strCond = "AND UM_USER_ID IN (SELECT RM_CREATED_BY FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & strDocNo & "' AND RM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>Your RFQ (" & strDocNo & ") has been rejected by " & objComp.CoyName & ".<BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.RFQReply ' vendor to buyer
        ''                    strSubject = "RFQ Reply"
        ''                    strCond = "AND UM_USER_ID IN (SELECT RM_CREATED_BY FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & strDocNo & "' AND RM_COY_ID = '" & strBCoyId & "')"
        ''                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Buyer, <BR>"
        ''                    strBody &= "<P>With reference to RFQ (" & strDocNo & "), "
        ''                    strBody &= objComp.CoyName & " has responded with a quotation (" & strDocNo2 & "). <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.CatalogueApproval ' from VendorAdmin to Purchasing Manager or Hub admin
        ''                    strSubject = "Catalogue Approval"
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    If pPara1 = "B" Then ' to buyer
        ''                        ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
        ''                        strBody = "Dear Purchasing Manager, <BR>"
        ''                    Else ' to Hub Admin
        ''                        blnAttn = True
        ''                        strEmailType = "1"
        ''                        strBody = "Dear Hub Admin, <BR>"
        ''                        strTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 
        ''                    End If

        ''                    strBody &= "<P>You have an outstanding Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " waiting for approval. <BR>"
        ''                    strBody &= "<P>For more details, please login to  " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.CatalogueDiscarded, EmailType.CatalogueRejected
        ''                    ' pPara1 = Discarded by whom: B - Buyer; H - HubAdmin
        ''                    ' pPara2 = Send to: B - Buyer; V - Vendor
        ''                    Dim str As String
        ''                    If pEmailType = EmailType.CatalogueDiscarded Then
        ''                        strSubject = "Catalogue Discarded"
        ''                        str = "discarded"
        ''                    Else
        ''                        strSubject = "Catalogue Rejected"
        ''                        str = "rejected"
        ''                    End If

        ''                    If pPara1 = "B" Then ' by Buyer to vendor
        ''                        objComp = objComps.GetCompanyDetails(strBCoyId)
        ''                        ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
        ''                        strBody = "Dear Vendor Admin, <BR>"
        ''                        strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been " & str & " by " & objComp.CoyName & ". <BR> "
        ''                    Else ' by Hub admin
        ''                        If pPara2 = "B" Then ' to buyer
        ''                            objComp = objComps.GetCompanyDetails(strSCoyId)
        ''                            ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
        ''                            strBody = "Dear Purchasing Manager, <BR>"
        ''                            strBody &= "<P>Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " has been " & str & " by Hub Admin. <BR>"
        ''                        Else ' to vendor
        ''                            ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
        ''                            strBody = "Dear Vendor Admin, <BR>"
        ''                            strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been " & str & " by Hub Admin. <BR>"
        ''                        End If
        ''                    End If

        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.CataloguePublished
        ''                    ' pPara1 = Send to: B - Buyer; V - Vendor
        ''                    strSubject = "Catalogue Published"

        ''                    If pPara1 = "B" Then ' to Buyer
        ''                        objComp = objComps.GetCompanyDetails(strSCoyId)
        ''                        ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
        ''                        strBody = "Dear Purchasing Manager, <BR>"
        ''                        strBody &= "<P>Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " has been published by Hub Admin. <BR>"
        ''                    Else ' to vendor
        ''                        ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
        ''                        strBody = "Dear Vendor Admin, <BR>"
        ''                        strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been published by Hub Admin. <BR>"
        ''                    End If

        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.ListPriceApproval
        ''                    strSubject = "List Price Approval"
        ''                    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''                    strBody = "Dear Hub Admin, <BR>"
        ''                    strBody &= "<P>You have an outstanding List Price item (" & strDocNo & ") from " & objComp.CoyName & " waiting for approval. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                    strTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 
        ''                    blnAttn = True
        ''                    strEmailType = "1"

        ''                Case EmailType.ListPricePublished, EmailType.ListPristRejected ', EmailType.ListPriceModified, EmailType.ListPriceDeleted
        ''                    Dim str As String
        ''                    Select Case pEmailType
        ''                        Case EmailType.ListPricePublished
        ''                            strSubject = "List Price Published"
        ''                            str = "published"
        ''                        Case EmailType.ListPristRejected
        ''                            strSubject = "List Price Rejected"
        ''                            str = "rejected"
        ''                            'Case EmailType.ListPriceModified
        ''                            '    strSubject = "List Price Modified"
        ''                            '    str = "modified"
        ''                            'Case EmailType.ListPriceDeleted
        ''                            '    strSubject = "List Price Deleted"
        ''                            '    str = "deleted"
        ''                    End Select

        ''                    ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
        ''                    strBody = "Dear Vendor Admin, <BR>"
        ''                    strBody &= "<P>Your List Price item (" & strDocNo & ") has been " & str & " by Hub Admin. <BR>"
        ''                    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"

        ''                Case EmailType.NewUserAccount
        ''                    strSubject = "New User Account Creation"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)

        ''                    ds = objUsers.getUserEmailList(strUserId, strBCoyId)

        ''                    strBody = "Dear " & objUser.Name & ", <BR>"
        ''                    strBody &= "<P>Your user account has been created by your Admin.<BR>"
        ''                    strBody &= "<P>To access the application, Please use the following login information.<BR><P>"

        ''                    strBody &= "Company Name               : " & objComp.CoyName
        ''                    strBody &= "<BR>Company ID             : " & objComp.CoyId
        ''                    strBody &= "<BR>User ID                : " & objUser.UserID
        ''                    strBody &= "<BR>New Temporary Password : " & pPara1
        ''                    strBody &= "<BR>Expiry Date            : " & objUser.NextExpiredDt & "<BR><P>"

        ''                    strBody &= "<P>To access, please login to " & EmailHomeEhubAddr & "<BR>"

        ''                Case EmailType.ChangePwd
        ''                    strSubject = "New User Password"
        ''                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
        ''                    objComp = objComps.GetCompanyDetails(strBCoyId)

        ''                    'If objComp.CoyType = "VENDOR" Or objComp.CoyType = "BOTH" Then
        ''                    '    ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strBCoyId)
        ''                    'End If
        ''                    'If objComp.CoyType = "BUYER" Or objComp.CoyType = "BOTH" Then
        ''                    '    ds = objUsers.getUserEmailList(FixedRole.Buyer_Administrator, strBCoyId)
        ''                    'End If
        ''                    ds = objUsers.getUserEmailList(strUserId, strBCoyId)

        ''                    'CC to Company 
        ''                    'Dim objDb As New EAD.DBCom
        ''                    'strsql = "SELECT CM_COY_NAME AS UM_USER_NAME, CM_COY_NAME,CM_EMAIL AS UM_EMAIL,CM_COY_ID AS UM_USER_ID " & _
        ''                    '         " FROM COMPANY_MSTR WHERE CM_COY_ID='" & strBCoyId & " ' AND CM_DELETED<>'Y' AND CM_STATUS='A'"
        ''                    'objDb.FillDsIn(ds, strsql, "CC")

        ''                    strBody = "Dear " & objUser.Name & ", <BR>"
        ''                    strBody &= "<P>Below is the newly generated temporary password based on your request."
        ''                    strBody &= "<P>Please use this temporary user password to login.<BR><P>"

        ''                    strBody &= "Company Name           : " & objComp.CoyName
        ''                    strBody &= "<BR>Company ID             : " & objComp.CoyId
        ''                    strBody &= "<BR>User ID                : " & objUser.UserID
        ''                    strBody &= "<BR>New Temporary Password : " & pPara1
        ''                    strBody &= "<BR>Expiry Date            : " & objUser.NextExpiredDt & "<BR>"
        ''            End Select

        ''            If Not ds Is Nothing Then
        ''                'Table 0/dt(0) will send as TO
        ''                If ds.Tables.Count > 0 Then
        ''                    If ds.Tables("COMPANY").Rows.Count > 0 Then
        ''                        If ds.Tables("COMPANY").Rows(0)("CM_DELETED") <> "Y" And ds.Tables("COMPANY").Rows(0)("CM_STATUS") = "A" Then
        ''                            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
        ''                                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
        ''                                    blnAttn = True
        ''                                    strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & "; "
        ''                                Else
        ''                                    dtr = dtAttn.NewRow()
        ''                                    dtr("UserId") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_ID"))
        ''                                    dtr("UserName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME"))
        ''                                    dtr("Email") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
        ''                                    dtr("CoyName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("CM_COY_NAME"))
        ''                                    dtAttn.Rows.Add(dtr)
        ''                                End If
        ''                            Next

        ''                            'kk.291204.Remark.Table 1/dt(1) will send as CC, blnAttn true to ensure have to otherwise will possible no TO but CC
        ''                            'to smbody
        ''                            If ds.Tables.Count = 3 And blnAttn Then
        ''                                For i = 0 To ds.Tables("USERCC").Rows.Count - 1
        ''                                    If Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
        ''                                        strCC &= Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) & "; "
        ''                                    Else
        ''                                        dtr = dtAttn.NewRow()
        ''                                        dtr("UserId") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_ID"))
        ''                                        dtr("UserName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME"))
        ''                                        dtr("Email") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL"))
        ''                                        dtr("CoyName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("CM_COY_NAME"))
        ''                                        dtAttn.Rows.Add(dtr)
        ''                                    End If
        ''                                Next
        ''                            End If
        ''                        Else ' company not active
        ''                            strEmailType = "4"
        ''                            GoTo GetEmailContent
        ''                        End If
        ''                    End If ' ds.Tables("COMPANY").Rows.Count > 0
        ''                End If ' ds.Tables.Count > 0
        ''            End If ' ds is nothing

        ''            If blnAttn Then
        ''                strEmailType = "1"
        ''            Else
        ''                If dtAttn.Rows.Count = 0 Then
        ''                    strEmailType = "3"
        ''                Else
        ''                    strEmailType = "2"
        ''                End If
        ''            End If

        ''GetEmailContent:
        ''            strLine = Strings.StrDup(EmailCompGen.Length, ".")
        ''            Select Case strEmailType
        ''                Case "1"
        ''                    MailTo = strTo
        ''                    MailCc = strCC
        ''                    Body = strBody & "<P>" & EmailFooter & EmailCompGen
        ''                    Subject = "eProcure : " & strSubject

        ''                Case "2"
        ''                    MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")  ' EmailHub
        ''                    strExtra = "Dear Hub Admin, <BR>"
        ''                    For i = 0 To dtAttn.Rows.Count - 1
        ''                        strExtra &= "User account for " & dtAttn.Rows(i)("UserId") & " (" & dtAttn.Rows(i)("UserId") & ", " & dtAttn.Rows(i)("Email") & ") of " & dtAttn.Rows(i)("CoyName") & " company has been deactivated. <BR>"
        ''                    Next

        ''                    Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & EmailFooter & EmailCompGen & "<BR><P>" & strLine
        ''                    Subject = "eProcure : Deactivated User - " & strSubject

        ''                Case "3"
        ''                    strExtra = "Dear Hub Admin, <BR>"
        ''                    strExtra &= "User not found from " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & ". <BR>"
        ''                    MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") 'EmailHub 

        ''                    Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & EmailFooter & EmailCompGen & "<BR><P>" & strLine
        ''                    Subject = "eProcure : No User Exists - " & strSubject

        ''                Case "4"
        ''                    strExtra = "Dear Hub Admin, <BR>"
        ''                    strExtra &= "<P>The company " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & " has been deactived. <BR> "
        ''                    MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 

        ''                    Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & EmailFooter & EmailCompGen & "<BR><P>" & strLine
        ''                    Subject = "eProcure : Deactivated Company - " & strSubject
        ''            End Select

        ''            SendMail()
        ''            objUser = Nothing
        ''            objUsers = Nothing
        ''            objComp = Nothing
        ''            objComps = Nothing
        ''        End Function

        ''Public Function sendItemUpdateMail(ByVal strSCoyId As String, ByVal strGroupIndex As String, ByVal dt1 As DataTable, ByVal dt2 As DataTable, ByVal blnList As Boolean, ByVal blnDiscount As Boolean)
        ''    ' strGroupIndex - Discount Group Code List
        ''    ' dt1 - New Catalogue Item added
        ''    ' dt2 - Existing Catalogue Item Updated
        ''    ' blnList = True id List Price has been modified
        ''    ' blnDiscount = True id Discount Price has been modified

        ''    Dim strSubject, strBody, strTo As String
        ''    Dim objComp As New Company
        ''    Dim objComps As New Companies
        ''    Dim objUser As New User
        ''    Dim objUsers As New Users
        ''    Dim ds As New DataSet
        ''    Dim i, intType As Integer

        ''    Dim dtAttn As New DataTable
        ''    dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
        ''    dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
        ''    dtAttn.Columns.Add("Email", Type.GetType("System.String"))
        ''    dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
        ''    Dim dtr As DataRow

        ''    strSubject = "Vendor Catalogue Item Update"
        ''    objComp = objComps.GetCompanyDetails(strSCoyId)

        ''    If blnList And blnDiscount Then
        ''        intType = 0
        ''    ElseIf blnList Then
        ''        intType = 1
        ''    ElseIf blnDiscount Then
        ''        intType = 2
        ''    End If

        ''    ds = objUsers.getPMEmailList(FixedRole.Purchasing_Manager, strSCoyId, intType, strGroupIndex)

        ''    If Not ds Is Nothing Then
        ''        For i = 0 To ds.Tables(0).Rows.Count - 1
        ''            strTo &= Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL")) & "; "
        ''        Next
        ''    End If ' ds is nothing

        ''    strBody = "Dear Purchasing Manager, <BR>"
        ''    strBody &= "<P>Vendor Company " & objComp.CoyName & " has added/updated the following catalogue item to their online catalogue: <BR>"
        ''    If dt1.Rows.Count > 0 Then
        ''        strBody &= "<P><U>New Catalogue Item Added:</U>"
        ''        strBody &= "<TABLE WIDTH='70%'>"
        ''        strBody &= "<TR><TD ALIGN='left' width='20%'><U>Item ID</U></TD><TD ALIGN='left' WIDTH='80%'><U>Item Description</U></TD></TR>"
        ''        For i = 0 To dt1.Rows.Count - 1
        ''            strBody &= "<TR>"
        ''            strBody &= "<TD valign='top'>" & dt1.Rows(i)("ItemId") & "</TD><TD ALIGN='left' valign='top'>" & dt1.Rows(i)("ItemDesc") & "</TD>"
        ''            strBody &= "</TR>"
        ''        Next
        ''        strBody &= "</TABLE>"
        ''    End If

        ''    If dt2.Rows.Count > 0 Then
        ''        strBody &= "<P><U>Existing Catalogue Item Updated:</U>"
        ''        strBody &= "<TABLE WIDTH='70%'>"
        ''        strBody &= "<TR><TD ALIGN='left' width='20%' ><U>Item ID</U></TD><TD ALIGN='left' WIDTH='50%'><U>Item Description</U></TD><TD ALIGN='left' WIDTH='15%' nowrap><U>List Price</U></TD><TD ALIGN='left' WIDTH='15%' nowrap><U>Discount Price</U></TD></TR>"
        ''        For i = 0 To dt2.Rows.Count - 1
        ''            strBody &= "<TR>"
        ''            strBody &= "<TD  valign='top'>" & dt2.Rows(i)("ItemId") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("ItemDesc") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("List") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("Discount") & "</TD>"
        ''            strBody &= "</TR>"
        ''        Next
        ''        strBody &= "</TABLE>"
        ''    End If

        ''    strBody &= "<P>For more details, please login to " & EmailHomeEhubAddr & " to view it. <BR>"
        ''    'strTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 

        ''    MailTo = strTo
        ''    Body = strBody & "<P>" & EmailFooter & EmailCompGen
        ''    Subject = "eProcure : " & strSubject
        ''    SendMail()
        ''    objUser = Nothing
        ''    objUsers = Nothing
        ''    objComp = Nothing
        ''    objComps = Nothing
        ''End Function
    End Class
End Namespace