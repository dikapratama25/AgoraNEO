'Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.
Imports SSO.Component
Imports System.Configuration

Namespace AgoraNEO
    Public Class Email

        Public Function SendMailByDS(ByVal ds As DataSet, ByVal strBody As String, ByVal strSubject As String, ByVal strRole As String)
            Dim i As Integer
            Dim strCC, strTo, strContent As String
            Dim blnAttn As Boolean = False
            Dim objMail As New AppMail
            Dim strExtra As String = ""
            Dim strLine As String
            Dim strEmailType As String
            Dim objCommon As New Common
            Dim objUsers As New Users

            Dim dtAttn As New DataTable
            dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
            dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
            dtAttn.Columns.Add("Email", Type.GetType("System.String"))
            dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
            Dim dtr As DataRow

            If Not ds Is Nothing Then
                'Table 0/dt(0) will send as TO
                If ds.Tables.Count > 0 Then
                    If ds.Tables("COMPANY").Rows.Count > 0 Then
                        If ds.Tables("COMPANY").Rows(0)("CM_DELETED") <> "Y" And ds.Tables("COMPANY").Rows(0)("CM_STATUS") = "A" Then
                            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
                                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
                                    blnAttn = True

                                    'Jules 2018.07.18 - ";" causing 'System.FormatException: The specified string is not in the form required for an e-mail address.'
                                    'strTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & "; "
                                    strTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))

                                    strContent = "Dear " & Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME")) & " (" & strRole & "), <BR>" & strBody

                                    objMail.MailTo = strTo
                                    If UCase(ds.Tables("COMPANY").Rows(0)("CM_COY_TYPE")) <> "BUYER" Then
                                        strCC = objUsers.getUserEmailCCList(ds.Tables("USERTO").Rows(i)("UM_COY_ID"), ds.Tables("USERTO").Rows(i)("UM_USER_ID"))

                                        If strCC <> "" Then
                                            objMail.MailCc = strCC
                                        End If
                                    End If

                                    objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                    '    objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen

                                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                    '    objMail.Body = strContent & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen

                                    'End If
                                    objMail.Subject = "Agora : " & strSubject
                                    objMail.SendMail()
                                Else
                                    dtr = dtAttn.NewRow()
                                    dtr("UserId") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_ID"))
                                    dtr("UserName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME"))
                                    dtr("Email") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
                                    dtr("CoyName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("CM_COY_NAME"))
                                    dtAttn.Rows.Add(dtr)
                                End If
                            Next

                            'kk.291204.Remark.Table 1/dt(1) will send as CC, blnAttn true to ensure have to otherwise will possible no TO but CC
                            'to smbody
                            If ds.Tables.Count = 3 And blnAttn Then
                                For i = 0 To ds.Tables("USERCC").Rows.Count - 1
                                    If Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then

                                        'Jules 2018.07.18 - ";" causing 'System.FormatException: The specified string is not in the form required for an e-mail address.'
                                        'strCC &= Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) & "; "
                                        strCC &= Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL"))

                                        strContent = "Dear " & Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME")) & ", <BR>" & strBody

                                        objMail.MailTo = strTo
                                        objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                        '    objMail.Body = strContent & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen

                                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                        '    objMail.Body = strContent & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen

                                        'End If
                                        objMail.Subject = "Agora : " & strSubject
                                        objMail.SendMail()
                                    Else
                                        dtr = dtAttn.NewRow()
                                        dtr("UserId") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_ID"))
                                        dtr("UserName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME"))
                                        dtr("Email") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL"))
                                        dtr("CoyName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("CM_COY_NAME"))
                                        dtAttn.Rows.Add(dtr)
                                    End If
                                Next
                            End If
                        Else ' company not active
                            strEmailType = "4"
                            GoTo GetEmailContent
                        End If
                    End If ' ds.Tables("COMPANY").Rows.Count > 0
                End If ' ds.Tables.Count > 0

                If Not blnAttn Then
                    If dtAttn.Rows.Count = 0 Then
                        strEmailType = "3"
                    Else
                        strEmailType = "2"
                    End If
                End If

GetEmailContent:
                strLine = Strings.StrDup(objCommon.EmailCompGen.Length, ".")
                If strEmailType <> "1" Then
                    Select Case strEmailType
                        Case "2"
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")  ' EmailHub
                            strExtra = "Dear Hub Admin, <BR>"
                            For i = 0 To dtAttn.Rows.Count - 1
                                strExtra &= "User account for " & dtAttn.Rows(i)("UserId") & " (" & dtAttn.Rows(i)("UserId") & ", " & dtAttn.Rows(i)("Email") & ") of " & dtAttn.Rows(i)("CoyName") & " company has been deactivated. <BR>"
                            Next

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'End If

                            objMail.Subject = "Agora : Deactivated User - " & strSubject
                            objMail.SendMail()
                        Case "3"
                            strExtra = "Dear Hub Admin, <BR>"
                            strExtra &= "User not found from " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & ". <BR>"
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") 'EmailHub 

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'End If
                            objMail.Subject = "Agora : No User Exists - " & strSubject
                            objMail.SendMail()
                        Case "4"
                            strExtra = "Dear Hub Admin, <BR>"
                            strExtra &= "<P>The company " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & " has been deactived. <BR> "
                            objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 

                            objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooterENT & objCommon.EmailCompGen & "<BR><P>" & strLine

                            'End If
                            objMail.Subject = "Agora : Deactivated Company - " & strSubject
                            objMail.SendMail()
                    End Select
                End If
            End If ' ds is nothing
        End Function

        Public Function sendNotification(ByVal pEmailType As EmailType, ByVal strUserId As String,
                                            ByVal strBCoyId As String, ByVal strSCoyId As String,
                                            ByVal strDocNo As String, ByVal strDocNo2 As String,
                                            Optional ByVal pPara1 As String = "",
                                            Optional ByVal pPara2 As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strsql As String
            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim strBody, strCC, strTo, strSubject As String
            Dim objUser As New User
            Dim objUsers As New Users
            Dim i, j As Integer
            Dim blnAttn As Boolean = False
            Dim objComp As New Company
            Dim objComps As New Companies
            Dim objMail As New AppMail
            Dim strCond As String
            Dim strExtra As String = ""
            Dim strLine As String
            Dim strEmailType As String
            Dim strRole As String
            Dim objaccess As New CheckAccess
            Dim comptype As String
            Dim sCompType As String = objComps.GetCompanyType(strBCoyId)
            If sCompType = "VENDOR" Then 'if login company is vendor company
                If strBCoyId <> "" Then
                    comptype = objaccess.getCompanyType(strBCoyId)

                Else
                    Dim objDb As New EAD.DBCom(ConfigurationManager.AppSettings("eProcurePath"))
                    Dim strsql1 As String = "SELECT DISTINCT POM_B_COY_ID FROM po_mstr WHERE POM_S_COY_ID='" & Common.Parse(strSCoyId) & "'"
                    ds1 = objDb.FillDs(strsql1)
                    If ds1.Tables(0).Rows.Count > 0 Then
                        For j = 0 To ds1.Tables(0).Rows.Count - 1
                            comptype = objaccess.getCompanyType(ds1.Tables(0).Rows(j).Item("POM_B_COY_ID"))
                            If comptype = "Buyer-Enterprise" Then
                                Exit For
                            End If
                        Next
                    Else
                        comptype = "Buyer-Enterprise"
                    End If
                End If
            Else    'If buyer company
                comptype = objaccess.getCompanyType(strBCoyId)
            End If
            'Web.HttpContext.Current.Session("CompanyType") = comptype
            ' strEmailType - 1 = active user exists
            '                2 = deactive user exists
            '                3 = no user exists
            '                4 = company not active

            strTo = ""
            Dim objCommon As New Common


            ' ai chu modified on 01/12/2005
            ' to include user name in the system notification
            Dim aa
            aa = "fdfd"

            Select Case pEmailType
                Case EmailType.AckGRN ' SK to 2nd 
                    strSubject = "GRN Acknowledgement"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    strCond = "AND UM_USER_ID IN (SELECT UL_USER_ID FROM GRN_MSTR LEFT JOIN DO_MSTR ON GM_DO_INDEX = DOM_DO_INDEX "
                    strCond &= "LEFT JOIN USERS_LOCATION ON UL_ADDR_CODE = DOM_D_ADDR_CODE AND UL_COY_ID = GM_B_COY_ID "
                    strCond &= "WHERE UL_LEVEL = 2 AND GM_GRN_NO = '" & strDocNo & "' AND GM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Second_Level_Receiver, strBCoyId, strCond)

                    'strBody = "Dear 2nd Level Storekeeper, <BR>"
                    strRole = "2nd Level Storekeeper"
                    strBody &= "<P>You have received a new GRN (" & strDocNo & ") from " & objUser.Name & ". <BR>"
                    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubAddr & " to acknowledge it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubAddr & " to acknowledge it. <BR>"
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubENTAddr & " to acknowledge it. <BR>"
                    'End If

                Case EmailType.DOCreated ' Vendor to Buyer
                    strSubject = "DO Created"
                    objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo2 & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>DO (" & strDocNo & ") has been created for PO (" & strDocNo2 & ") by " & objComp.CoyName & ". <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
                    'End If

                Case EmailType.DOCreatedToSK ' Vendor to SK
                    strSubject = "DO Created"
                    objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor
                    strCond = "AND UGM_FIXED_ROLE = 'Store Keeper' AND UM_USER_ID IN (SELECT UL_USER_ID FROM DO_MSTR "
                    strCond &= "INNER JOIN PO_MSTR ON DOM_PO_INDEX=POM_PO_INDEX " 'Jules 2018.11.13 - To ensure only requester will receive this email.
                    strCond &= "LEFT JOIN USERS_LOCATION ON UL_ADDR_CODE = DOM_D_ADDR_CODE AND UL_COY_ID = '" & strBCoyId & "' AND POM_BUYER_ID=UL_USER_ID "

                    'Jules 2018.10.08 - Added SCoyID filter
                    'strCond &= "WHERE UL_LEVEL = 1 AND DOM_DO_NO = '" & strDocNo & "' )"
                    strCond &= "WHERE UL_LEVEL = 1 AND DOM_DO_NO = '" & strDocNo & "' AND DOM_S_COY_ID = '" & strSCoyId & "')"
                    'End modification.

                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Storekeeper, <BR>"
                    strRole = "Storekeeper"
                    strBody &= "<P>DO (" & strDocNo & ") has been created for PO (" & strDocNo2 & ") by " & objComp.CoyName & ". <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.GoodsReceiptNoteCreated 'Buyer to Vendor
                    ' pPara1 = GRN No
                    strSubject = "GRN Created"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    objComp = objComps.GetCompanyDetails(strBCoyId) ' buyer 
                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

                    'strBody = "Dear Vendor, <BR>"
                    strRole = "Vendor"
                    strBody &= "<P>Goods from DO (" & strDocNo & ") for PO (" & strDocNo2 & ") have been received and accepted by the buyer. <BR>"
                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has created a GRN (" & pPara1 & ") "
                    strBody &= "for PO (" & strDocNo2 & ") on " & FormatDateTime(Today, Microsoft.VisualBasic.DateFormat.ShortDate) & ".<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.GoodsReceiptNoteReject 'Buyer to Vendor
                    ' pPara1 = GRN No
                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)
                    strSubject = "GRN Created With Rejection"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    objComp = objComps.GetCompanyDetails(strBCoyId)

                    'strBody = "Dear Vendor, <BR>"
                    strRole = "Vendor"
                    strBody &= "<P>Goods from your DO (" & strDocNo & ") for PO (" & strDocNo2 & ") have been rejected. <BR>"
                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has created a GRN (" & pPara1 & ") "
                    strBody &= "with rejection for PO (" & strDocNo2 & ") <BR>on " & FormatDateTime(Today, Microsoft.VisualBasic.DateFormat.ShortDate) & ".<BR>"
                    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.InvoiceApproval ' Buyer - Finance Officer to Finance Manager
                    strSubject = "Invoice Approval"
                    ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId)

                    'strBody = "Dear Finance Manager, <BR>"
                    strRole = "Approving Officer"
                    strBody &= "<P>You have an outstanding Invoice (" & strDocNo & ") waiting for approval. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.InvoiceCreated ' Vendor to Buyer(Finance Officer)
                    strSubject = "Invoice Created"
                    'ds = objUsers.getUserEmailList(FixedRole.Finance_Officer, strBCoyId)
                    '  ds = objUsers.getFirstFOEmailList(strBCoyId, strDocNo)
                    'Michelle (29/8/2007) - To send email to all FOs if there is no multiple invoice approval
                    Dim strSql1, strInvAppr As String
                    Dim objDB As New EAD.DBCom

                    strSql1 = "Select CM_INV_APPR from COMPANY_MSTR where CM_COY_ID = '" & strBCoyId & "'"
                    Dim tDS As DataSet = objDB.FillDs(strSql1)

                    If tDS.Tables(0).Rows.Count > 0 Then
                        strInvAppr = tDS.Tables(0).Rows(0).Item("CM_INV_APPR")
                    End If
                    If strInvAppr = "Y" Then
                        ds = objUsers.getFirstFOEmailList(strBCoyId, strDocNo, strSCoyId)
                    Else
                        ds = objUsers.getUserEmailList(FixedRole.Finance_Officer, strBCoyId)
                    End If

                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Finance Officer, <BR>"
                    strRole = "Finance Officer"
                    strBody &= "<P>You have received an invoice (" & strDocNo & ") created by vendor " & objComp.CoyName & " for PO (" & strDocNo2 & ") .<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.POAccepted ' Vendor to Buyer
                    strSubject = "PO Accepted"
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Buyer/Consolidator, <BR>"
                    strRole = "Buyer/Consolidator"
                    strBody &= "<P>Your PO (" & strDocNo & ") has been accepted by " & objComp.CoyName & ".<BR>"
                    strBody &= "<P>Requestor Name : " & strDocNo2 & "<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.POCancellationRequest ' Buyer to Vendor
                    Dim IsBA As Boolean
                    'Michelle (6/2/2010) - Check whether user is Buyer Admin
                    'If HttpContext.Current.Session("Env") <> "FTN" Then
                    If blnEnterpriseVersion = True Then
                        IsBA = objUsers.BAdminRole(strUserId, strBCoyId)
                    Else
                        IsBA = False
                    End If
                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)
                    strSubject = "PO Cancellation Request"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    objComp = objComps.GetCompanyDetails(strBCoyId)

                    'strBody = "Dear Vendor, <BR>"
                    strRole = "Vendor"

                    If IsBA Then
                        strBody &= "<P>Buyer Administrator " & objUser.Name & " of " & objComp.CoyName & " has requested to cancel PO (" & strDocNo & "). <BR>"
                    Else
                        strBody &= "<P>Buyer " & objUser.Name & " of " & objComp.CoyName & " has requested to cancel PO (" & strDocNo & "). <BR>"
                    End If
                    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubAddr & " to respond to the request. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubAddr & " to respond to the request. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>Please login to " & objCommon.EmailHomeEhubENTAddr & " to respond to the request. <BR>"

                    'End If

                Case EmailType.POCancellationRequestToAOBuyer 'Michelle (eBiz/134/08) - To AOs (eBiz/134/08) 
                    'Michelle (6/2/2010) - To Buyer if it is cancelled by Buyer Administrator
                    Dim strsql2 As String
                    Dim strsql1 As String
                    Dim strAOsEmail As String
                    Dim dsAOeMail As New DataSet
                    Dim objDb As New EAD.DBCom
                    Dim IsBA As Boolean
                    Dim strPRNo As String
                    Dim strTemp As String

                    'If HttpContext.Current.Session("Env") <> "FTN" Then
                    If blnEnterpriseVersion = True Then
                        IsBA = objUsers.BAdminRole(strUserId, strBCoyId) 'Michelle (6/2/2010) - Check whether user is Buyer Admin
                    Else
                        IsBA = False
                    End If
                    If pPara2 = "AO" Then
                        strsql2 = "SELECT PRD_PR_NO FROM pr_details " _
                            & "INNER JOIN pr_mstr ON PRM_PR_NO=PRD_PR_NO " _
                            & "WHERE PRD_CONVERT_TO_IND='PO' AND prd_convert_to_doc = '" & Common.Parse(strDocNo) & "' " _
                            & "AND PRM_PR_TYPE='CC' AND PRM_COY_ID='" & Common.Parse(strBCoyId) & "'"
                        strPRNo = objDb.GetVal(strsql2)
                        If strPRNo <> "" Then   'Convert from Contract PR
                            strsql1 = "SELECT DISTINCT(UM_EMAIL) AS Email "
                            strsql1 &= "FROM PR_APPROVAL, USER_MSTR, pr_mstr "
                            strsql1 &= "WHERE PRA_PR_INDEX=PRM_PR_Index AND PRM_PR_NO = '" & Common.Parse(strPRNo) & "' "
                            strsql1 &= "AND PRM_COY_ID = '" & Common.Parse(strBCoyId) & "' "
                            strsql1 &= "AND PRA_FOR = 'PR'  AND PRA_ACTIVE_AO IS NOT NULL "
                            strsql1 &= "AND PRA_ACTIVE_AO = UM_USER_ID AND UM_DELETED <> 'Y'"

                        Else
                            strsql1 = "select DISTINCT(UM_EMAIL) as Email from PR_APPROVAL, USER_MSTR, PO_MSTR "
                            strsql1 &= "where POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "' AND "
                            strsql1 &= "POM_PO_INDEX = PRA_PR_INDEX AND PRA_FOR = 'PO'  AND PRA_ACTIVE_AO IS NOT NULL and PRA_ACTIVE_AO = UM_USER_ID AND UM_DELETED <> 'Y'"
                        End If
                        strTemp = objDb.GetVal(strsql1)
                        If strTemp = "" Then
                            strsql1 = "SELECT DISTINCT(UM_EMAIL) AS Email "
                            strsql1 &= "FROM PR_APPROVAL, USER_MSTR, pr_mstr "
                            strsql1 &= "WHERE PRA_PR_INDEX=PRM_PR_Index AND PRM_PR_NO = "
                            strsql1 &= "(SELECT DISTINCT PRM_PR_NO FROM pr_mstr "
                            strsql1 &= "INNER JOIN po_details ON POD_PR_INDEX=PRM_PR_Index "
                            strsql1 &= "WHERE POD_PO_NO='" & Common.Parse(strDocNo) & "' "
                            strsql1 &= "AND POD_COY_ID = '" & Common.Parse(strBCoyId) & "') "
                            strsql1 &= "AND PRM_COY_ID = '" & Common.Parse(strBCoyId) & "' "
                            strsql1 &= "AND PRA_FOR = 'PR'  AND PRA_ACTIVE_AO IS NOT NULL "
                            strsql1 &= "AND PRA_ACTIVE_AO = UM_USER_ID AND UM_DELETED <> 'Y'"
                        End If

                        'strsql1 = "select DISTINCT(UM_EMAIL) as Email from PR_APPROVAL, USER_MSTR, PO_MSTR "
                        'strsql1 &= "where POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "' AND "
                        'strsql1 &= "POM_PO_INDEX = PRA_PR_INDEX AND PRA_FOR = 'PO'  AND PRA_ACTIVE_AO IS NOT NULL and PRA_ACTIVE_AO = UM_USER_ID AND UM_DELETED <> 'Y'"

                        dsAOeMail = objDb.FillDs(strsql1)
                        If dsAOeMail.Tables(0).Rows.Count > 0 Then
                            blnAttn = True
                            For i = 0 To dsAOeMail.Tables(0).Rows.Count - 1
                                strAOsEmail &= dsAOeMail.Tables(0).Rows(i)("Email") & ";"
                            Next
                            strSubject = "Agora : PO Cancellation Request"
                            objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                            objComp = objComps.GetCompanyDetails(strSCoyId)

                            strBody = "Dear Approving Officer, <BR>"

                            'Michelle (6/2/2010) - Check whether cancel by Buyer Administrator
                            If IsBA Then
                                strBody &= "<P>Buyer Administrator " & objUser.Name & " has requested a PO Cancellation Request to "

                            Else
                                strBody &= "<P>Buyer " & objUser.Name & " has requested a PO Cancellation Request to "
                            End If

                            strBody &= objComp.CoyName & " for PO (" & strDocNo & "). <BR>"
                            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                            'End If
                            objMail.Subject = strSubject
                            objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                            'objMail.Body = strBody & "<P>"
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body &= objCommon.EmailFooter
                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body &= objCommon.EmailFooterENT
                            'End If
                            'objMail.Body &= objCommon.EmailCompGen
                            objMail.MailBCc = strAOsEmail
                            objMail.SendMail()
                            objMail = Nothing
                        End If
                    Else 'To Buyer
                        'To get the PO
                        strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                        ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                        strSubject = "PO Cancellation Request"
                        objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                        objComp = objComps.GetCompanyDetails(strSCoyId)
                        strRole = "Buyer"
                        strBody &= "<P>Buyer Administrator " & objUser.Name & " has requested a PO Cancellation Request to "
                        strBody &= objComp.CoyName & " for PO (" & strDocNo & "). <BR>"
                        strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                        'End If
                    End If

                Case EmailType.POCancellationRequestToAO 'Michelle (14/12/2010) - To AOs only ie. before sending to vendor
                    Dim strsql1 As String
                    Dim strAOsEmail As String
                    Dim dsAOeMail As New DataSet
                    Dim objDb As New EAD.DBCom
                    Dim IsBA As Boolean
                    'If HttpContext.Current.Session("Env") <> "FTN" Then
                    If blnEnterpriseVersion = True Then
                        IsBA = objUsers.BAdminRole(strUserId, strBCoyId) 'Michelle (6/2/2010) - Check whether user is Buyer Admin
                    Else
                        IsBA = False
                    End If
                    If pPara2 = "AO" Then
                        strsql1 = "select DISTINCT(UM_EMAIL) as Email from PR_APPROVAL, USER_MSTR, PO_MSTR "
                        strsql1 &= "where POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "' AND "
                        strsql1 &= "POM_PO_INDEX = PRA_PR_INDEX AND PRA_FOR = 'PO' and PRA_AO = UM_USER_ID AND UM_DELETED <> 'Y'"

                        dsAOeMail = objDb.FillDs(strsql1)
                        If dsAOeMail.Tables(0).Rows.Count > 0 Then
                            blnAttn = True
                            For i = 0 To dsAOeMail.Tables(0).Rows.Count - 1
                                strAOsEmail &= dsAOeMail.Tables(0).Rows(i)("Email") & ";"
                            Next
                            strSubject = "Agora : PO Cancellation Request"
                            objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                            objComp = objComps.GetCompanyDetails(strSCoyId)

                            strBody = "Dear Approving Officer, <BR>"

                            'Michelle (6/2/2010) - Check whether cancel by Buyer Administrator
                            If IsBA Then
                                strBody &= "<P>Buyer Administrator " & objUser.Name & " has requested a PO Cancellation Request to "

                            Else
                                strBody &= "<P>Buyer " & objUser.Name & " has requested a PO Cancellation Request to "
                            End If

                            strBody &= objComp.CoyName & " for PO (" & strDocNo & "). <BR>"
                            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                            'End If
                            objMail.Subject = strSubject
                            objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                            'objMail.Body = strBody & "<P>"
                            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                            '    objMail.Body &= objCommon.EmailFooter
                            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                            '    objMail.Body &= objCommon.EmailFooterENT
                            'End If
                            'objMail.Body &= objCommon.EmailCompGen
                            objMail.MailBCc = strAOsEmail
                            objMail.SendMail()
                            objMail = Nothing
                        End If
                    Else 'To Buyer
                        'To get the PO
                        strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                        ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                        strSubject = "PO Cancellation Request"
                        objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                        objComp = objComps.GetCompanyDetails(strSCoyId)
                        strRole = "Buyer"
                        strBody &= "<P>Buyer Administrator " & objUser.Name & " has requested a PO Cancellation Request to "
                        strBody &= objComp.CoyName & " for PO (" & strDocNo & "). <BR>"
                        strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                        'End If
                    End If

                Case EmailType.AckPOCancellationRequest ' Vendor to Buyer
                    ' Dim objUserRole As New UserRoles
                    Dim objPO As New PurchaseOrder

                    'strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    'Michelle (6/2/2010) - To check who to send to
                    '  If pPara1 = "BACancel" Or pPara1 = "ToBuyer" Then
                    If pPara1 = "BACancel" Then
                        strCond = "AND UM_USER_ID = '" & pPara2 & "' AND UM_COY_ID = '" & strBCoyId & "'"
                    Else
                        strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    End If

                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                    strSubject = "PO Cancellation Request - Acknowledgement"
                    objUser = objUsers.GetUserDetails(strUserId, strSCoyId)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    If pPara1 = "BACancel" Then
                        strRole = "Buyer Administrator"
                    Else
                        strRole = "Buyer/Consolidator"
                    End If

                    If pPara1 = "BACancel" Or pPara1 = "BuyerCancel" Then
                        strBody &= "<P>Your cancellation request for PO (" & strDocNo & ") has been acknowledged by " & objUser.Name & " from " & objComp.CoyName & ". <BR>"
                    Else
                        strBody &= "<P>Buyer Administrator cancellation request for PO (" & strDocNo & ") has been acknowledged by " & objUser.Name & " from " & objComp.CoyName & ". <BR>"
                    End If

                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.POCreated ' to buyer
                    strSubject = "PO Created"
                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                    '       objComp = objComps.GetCompanyDetails(strSCoyId) ' vendor

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    'Michelle (12/7/2007) - To cater for multiple vendors
                    ' strBody &= "<P>Your PR (" & strDocNo & ") has been converted into a PO (" & strDocNo2 & ") and sent to " & objComp.CoyName & " company. <BR>"
                    ' strBody &= "<P>Requestor Name : " & pPara1 & "<BR>"
                    ' strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    strBody &= "<P>Your PR (" & strDocNo & ") has been converted into PO (" & strDocNo2 & ") and sent to " & pPara2 & ". <BR>"
                    strBody &= "<P>Requestor Name : " & pPara1 & "<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.PORaised ' Buyer to Vendor
                    strSubject = "PO Raised"
                    objComp = objComps.GetCompanyDetails(strBCoyId)
                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

                    'strBody = "Dear Vendor, <BR>"
                    strRole = "Vendor"
                    strBody &= "<P>You have received a new PO (" & strDocNo & ") from " & objComp.CoyName & " Buyer. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.POApproved
                    strSubject = "PO Approved"
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Approving Officer, <BR>"
                    strRole = "Purchaser"
                    strBody &= "<P>Your PO (" & strDocNo & ") has been approved.<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.PORejected ' Vendor to Buyer (PO owner)
                    strSubject = "PO Rejected"
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Approving Officer, <BR>"
                    'Michelle (28/4/2011) - to PO owner
                    strRole = "Approving Officer"
                    'strBody &= "<P>Your PO (" & strDocNo & ") has been rejected.<BR>"
                    strBody &= "<P>Your PO (" & strDocNo & ") has been rejected by " & objComp.CoyName & ".<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.PRConsolidated
                    strSubject = "PR Consolidation"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    strCond = "AND UM_USER_ID IN (SELECT PRM_CONSOLIDATOR FROM PR_MSTR "
                    strCond &= "WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Consolidator, strBCoyId, strCond)

                    'strBody = "Dear Consolidator, <BR>"
                    strRole = "Consolidator"
                    strBody &= "<P>You have a PR (" & strDocNo & ") waiting for consolidation. <BR>"
                    strBody &= "<P>Please go to " & objCommon.EmailHomeAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>Please go to " & objCommon.EmailHomeAddr & " to view it. <BR>"
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>Please go to " & objCommon.EmailHomeENTAddr & " to view it. <BR>"
                    'End If

                Case EmailType.PRCancelled ' Buyer to AO
                    Dim strSqlCond As String
                    Dim objDB As New EAD.DBCom

                    strSubject = "PR Cancelled"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    'Michelle (22/9/2010) - To change the TOP 1 syntax
                    'strCond = "AND UM_USER_ID IN (SELECT TOP 1 PRA_AO FROM PR_MSTR LEFT JOIN PR_APPROVAL ON PRM_PR_INDEX = PRA_PR_INDEX "
                    'strCond &= "WHERE PRM_PR_NO = '" & strDocNo & "' "
                    'strCond &= "AND PRM_COY_ID = '" & strBCoyId & "' "
                    'strCond &= "ORDER BY PRA_SEQ)"
                    strSqlCond = " LEFT JOIN PR_APPROVAL ON PRM_PR_INDEX = PRA_PR_INDEX WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "' ORDER BY PRA_SEQ"
                    If pPara1 = "All" Then
                        strCond &= "AND UM_USER_ID IN (SELECT PRA_AO FROM PR_MSTR " & strSqlCond & ")"
                    Else
                        strCond &= "AND UM_USER_ID IN ('" & objDB.Get1Column("PR_MSTR", "PRA_AO", strSqlCond) & "')"
                    End If

                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)

                    'strBody = "Dear Approving Officer, <BR>"
                    strRole = "Approving Officer"
                    strBody &= "<P>PR (" & strDocNo & ") has been cancelled by " & objUser.Name & ". <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If


                Case EmailType.PRCancelledToBuyer
                    strSubject = "PR Cancelled"
                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    strRole = "Buyer"
                    strBody &= "<P>PR (" & strDocNo & ") has been cancelled by your Purchasing Officer. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.PRHeld ' AO to Buyer
                    strSubject = "PR Held"
                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>Your Approving Officer has put your PR (" & strDocNo & ") on hold. <BR>"
                    strBody &= "<P>Requestor Name : " & strDocNo2 & "<BR>"
                    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.POHeld ' AO to Buyer
                    strSubject = "PO Held"
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>Your Approving Officer has put your PO (" & strDocNo & ") on hold. <BR>"
                    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.PRRejected ' AO to Buyer
                    strSubject = "PR Rejected"
                    strCond = "AND UM_USER_ID IN (SELECT PRM_BUYER_ID FROM PR_MSTR WHERE PRM_PR_NO = '" & strDocNo & "' AND PRM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>Your PR (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
                    strBody &= "<P>Requestor Name : " & strDocNo2 & "<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.PORejectedBy ' AO to Buyer
                    strSubject = "PO Rejected"
                    strCond = "AND UM_USER_ID IN (SELECT POM_BUYER_ID FROM PO_MSTR WHERE POM_PO_NO = '" & strDocNo & "' AND POM_B_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Purchaser"
                    strBody &= "<P>Your PO (" & strDocNo & ") has been rejected by your Approving Officer. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.RFQRequested ' Buyer to Vendor
                    ' strdocNo2 = expired date
                    strSubject = "Quotation Requested"
                    objComp = objComps.GetCompanyDetails(strBCoyId)
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId)

                    'strBody = "Dear Vendor, <BR>"
                    strRole = "Vendor"
                    strBody &= "<P>" & objUser.Name & " from " & objComp.CoyName & " has requested a RFQ (" & strDocNo & ") which will expired on " & strDocNo2 & ". <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.RFQRejected ' vendor to buyer
                    strSubject = "RFQ Rejected"
                    strCond = "AND UM_USER_ID IN (SELECT RM_CREATED_BY FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & strDocNo & "' AND RM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>Your RFQ (" & strDocNo & ") has been rejected by " & objComp.CoyName & ".<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.RFQReply ' vendor to buyer
                    strSubject = "RFQ Reply"
                    strCond = "AND UM_USER_ID IN (SELECT RM_CREATED_BY FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & strDocNo & "' AND RM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>With reference to RFQ (" & strDocNo & "), "
                    strBody &= objComp.CoyName & " has responded with a quotation (" & strDocNo2 & "). <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.RFQSupply ' vendor to reject
                    strSubject = "RFQ Unable to Supply"
                    strCond = "AND UM_USER_ID IN (SELECT RM_CREATED_BY FROM RFQ_MSTR WHERE RM_RFQ_NO = '" & strDocNo & "' AND RM_COY_ID = '" & strBCoyId & "')"
                    ds = objUsers.getUserEmailList(FixedRole.Buyer, strBCoyId, strCond)
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    'strBody = "Dear Buyer, <BR>"
                    strRole = "Buyer"
                    strBody &= "<P>With reference to RFQ (" & strDocNo & "), "
                    strBody &= objComp.CoyName & " has rejected the request. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.CatalogueApproval ' from VendorAdmin to Purchasing Manager or Hub admin
                    strSubject = "Catalogue Approval"
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    If pPara1 = "B" Then ' to buyer
                        ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
                        'strBody = "Dear Purchasing Manager, <BR>"
                        strRole = "Purchasing Manager"
                        strBody &= "<P>You have an outstanding Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " waiting for approval. <BR>"
                        strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                        'End If

                    Else ' to Hub Admin
                        blnAttn = True
                        strEmailType = "1"
                        strTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 
                        strBody = "Dear Hub Admin, <BR>"
                        strBody &= "<P>You have an outstanding Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " waiting for approval. <BR>"
                        strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    strBody &= "<P>For more details, please login to  " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                        'End If

                        objMail.MailTo = strTo
                        objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                        'objMail.Body = strBody & "<P>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    objMail.Body &= objCommon.EmailFooter
                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    objMail.Body &= objCommon.EmailFooterENT
                        'End If
                        'objMail.Body &= objCommon.EmailCompGen
                        objMail.Subject = "Agora : " & strSubject
                        objMail.SendMail()
                    End If

                Case EmailType.CatalogueDiscarded, EmailType.CatalogueRejected
                    ' pPara1 = Discarded by whom: B - Buyer; H - HubAdmin
                    ' pPara2 = Send to: B - Buyer; V - Vendor
                    Dim str As String
                    If pEmailType = EmailType.CatalogueDiscarded Then
                        strSubject = "Catalogue Discarded"
                        str = "discarded"
                    Else
                        strSubject = "Catalogue Rejected"
                        str = "rejected"
                    End If

                    If pPara1 = "B" Then ' by Buyer to vendor
                        objComp = objComps.GetCompanyDetails(strBCoyId)
                        ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
                        'strBody = "Dear Vendor Admin, <BR>"
                        strRole = "Vendor Admin"
                        strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been " & str & " by " & objComp.CoyName & ". <BR> "
                    Else ' by Hub admin
                        If pPara2 = "B" Then ' to buyer
                            objComp = objComps.GetCompanyDetails(strSCoyId)
                            ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
                            'strBody = "Dear Purchasing Manager, <BR>"
                            strRole = "Purchasing Manager"
                            strBody &= "<P>Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " has been " & str & " by Hub Admin. <BR>"
                        Else ' to vendor
                            ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
                            'strBody = "Dear Vendor Admin, <BR>"
                            strRole = "Vendor Admin"
                            strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been " & str & " by Hub Admin. <BR>"
                        End If
                    End If

                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.CataloguePublished
                    ' pPara1 = Send to: B - Buyer; V - Vendor
                    strSubject = "Catalogue Published"

                    If pPara1 = "B" Then ' to Buyer
                        objComp = objComps.GetCompanyDetails(strSCoyId)
                        ds = objUsers.getUserEmailList(FixedRole.Purchasing_Manager, strBCoyId)
                        'strBody = "Dear Purchasing Manager, <BR>"
                        strRole = "Purchasing Manager"
                        strBody &= "<P>Contract Catalogue Group (" & strDocNo & ") from " & objComp.CoyName & " has been published by Hub Admin. <BR>"
                    Else ' to vendor
                        ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
                        'strBody = "Dear Vendor Admin, <BR>"
                        strRole = "Vendor Admin"
                        strBody &= "<P>Your Contract Catalogue Group (" & strDocNo & ") has been published by Hub Admin. <BR>"
                    End If

                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If

                Case EmailType.ListPriceApproval
                    strSubject = "List Price Approval"
                    objComp = objComps.GetCompanyDetails(strSCoyId)

                    strBody = "Dear Hub Admin, <BR>"
                    strBody &= "<P>You have an outstanding List Price item (" & strDocNo & ") from " & objComp.CoyName & " waiting for approval. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If
                    strTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 
                    blnAttn = True
                    strEmailType = "1"

                    objMail.MailTo = strTo
                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                    'objMail.Body = strBody & "<P>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    objMail.Body &= objCommon.EmailFooter
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    objMail.Body &= objCommon.EmailFooterENT
                    'End If
                    'objMail.Body &= objCommon.EmailCompGen
                    objMail.Subject = "Agora : " & strSubject
                    objMail.SendMail()

                Case EmailType.ListPricePublished, EmailType.ListPristRejected ', EmailType.ListPriceModified, EmailType.ListPriceDeleted
                    Dim str As String
                    Select Case pEmailType
                        Case EmailType.ListPricePublished
                            strSubject = "List Price Published"
                            str = "published"
                        Case EmailType.ListPristRejected
                            strSubject = "List Price Rejected"
                            str = "rejected"
                            'Case EmailType.ListPriceModified
                            '    strSubject = "List Price Modified"
                            '    str = "modified"
                            'Case EmailType.ListPriceDeleted
                            '    strSubject = "List Price Deleted"
                            '    str = "deleted"
                    End Select

                    ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strSCoyId)
                    'strBody = "Dear Vendor Admin, <BR>"
                    strRole = "Vendor Admin"
                    strBody &= "<P>Your List Price item (" & strDocNo & ") has been " & str & " by Hub Admin. <BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If


                    'For eRFP Application
                    'created by esther on 03/01/2006


                Case EmailType.ApprovedVCoyReg

                    'When eAdmin approved Vendor Company Registration email will send to vendor to inform Registration approval.

                    strSubject = "Vendor Company Registration"

                    Dim objDb As New EAD.DBCom
                    Dim strStatus As String

                    Select Case pPara1
                        Case "1"
                            objComp = objComps.GetCompanyDetails(strSCoyId)
                            strStatus = "Approved"
                            strBody &= "<P> Dear " & objComp.CoyName & " <BR>"
                            strBody &= "<P>Your company registration has been " & strStatus & " by HUB ADMIN. You will receive new account user id and temp password from HUB Admin<BR>"
                            strCond = "SELECT CM_EMAIL FROM COMPANY_MSTR WHERE CM_COY_ID='" & strSCoyId & "'"

                        Case "2"
                            strStatus = "Reject"
                            objComp = objComps.GetDetails(strSCoyId)
                            strBody &= "<P>Your company registration has been " & strStatus & " by HUB ADMIN. <BR>"
                            strCond = "SELECT CM_EMAIL FROM COMPANY_MSTR_TEMP WHERE CM_COY_ID='" & strSCoyId & "'"
                    End Select


                    strTo = objComp.Email ' objdb.GetVal(strCond)

                    strRole = "Vendor Company"


                    blnAttn = True
                    strEmailType = "1"



                    objMail.MailTo = strTo
                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                    'objMail.Body = strBody & "<P>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    objMail.Body &= objCommon.EmailFooter
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    objMail.Body &= objCommon.EmailFooterENT
                    'End If
                    'objMail.Body &= objCommon.EmailCompGen
                    objMail.Subject = "eRFP : " & strSubject
                    objMail.SendMail()
                    strBody = objMail.Body
                    strTo = objComp.Email ' objdb.GetVal(strCond)



                    'strBody &= "<P>Please go to " & objCommon.EmailHomeeRFPAddr & " to view it. <BR>"



                Case EmailType.NewUserAccount
                    '       If pEmailType = 33 Then
                    strSubject = "New User Account Creation"
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    objComp = objComps.GetCompanyDetails(strBCoyId)
                    blnAttn = True
                    ds = objUsers.getUserEmailList(strUserId, strBCoyId)

                    strBody = "Dear " & objUser.Name & ", <BR>"
                    strBody &= "<P>Your user account has been created by your Admin.<BR>"
                    strBody &= "<P>To access the application, Please use the following login information.<BR><P>"

                    strBody &= "Company Name               : " & objComp.CoyName
                    strBody &= "<BR>Company ID             : " & objComp.CoyId
                    strBody &= "<BR>User ID                : " & objUser.UserID
                    strBody &= "<BR>New Temporary Password : " & pPara1
                    strBody &= "<BR>Expiry Date            : " & objUser.NextExpiredDt & "<BR><P>"

                    strBody &= "<P>To access, please login to " & objCommon.EmailHomeEhubAddr & "<BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= "<P>To access, please login to " & objCommon.EmailHomeEhubAddr & "<BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= "<P>To access, please login to " & objCommon.EmailHomeEhubENTAddr & "<BR>"

                    'End If
                    objMail.MailTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                    'objMail.Body = strBody & "<P>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    objMail.Body &= objCommon.EmailFooter
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    objMail.Body &= objCommon.EmailFooterENT
                    'End If
                    'objMail.Body &= objCommon.EmailCompGen
                    objMail.Subject = "Agora : " & strSubject
                    objMail.SendMail()

                    '            End If

                Case EmailType.ChangePwd
                    strSubject = "Change Password" 'New User
                    objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
                    objComp = objComps.GetCompanyDetails(strBCoyId)

                    'If objComp.CoyType = "VENDOR" Or objComp.CoyType = "BOTH" Then
                    '    ds = objUsers.getUserEmailList(FixedRole.Vendor_Administrator, strBCoyId)
                    'End If
                    'If objComp.CoyType = "BUYER" Or objComp.CoyType = "BOTH" Then
                    '    ds = objUsers.getUserEmailList(FixedRole.Buyer_Administrator, strBCoyId)
                    'End If
                    blnAttn = True
                    ds = objUsers.getUserEmailList(strUserId, strBCoyId)

                    'CC to Company 
                    'Dim objDb As New EAD.DBCom
                    'strsql = "SELECT CM_COY_NAME AS UM_USER_NAME, CM_COY_NAME,CM_EMAIL AS UM_EMAIL,CM_COY_ID AS UM_USER_ID " & _
                    '         " FROM COMPANY_MSTR WHERE CM_COY_ID='" & strBCoyId & " ' AND CM_DELETED<>'Y' AND CM_STATUS='A'"
                    'objDb.FillDsIn(ds, strsql, "CC")

                    strBody = "Dear " & objUser.Name & ", <BR>"
                    strBody &= "<P>Below is the newly generated temporary password based on your request."
                    strBody &= "<P>Please use this temporary user password to login.<BR><P>"

                    strBody &= "Company Name           : " & objComp.CoyName
                    strBody &= "<BR>Company ID             : " & objComp.CoyId
                    strBody &= "<BR>User ID                : " & objUser.UserID
                    strBody &= "<BR>New Temporary Password : " & pPara1
                    strBody &= "<BR>Expiry Date            : " & objUser.NextExpiredDt & "<BR>"

                    objMail.MailTo = objUser.Email
                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                    'objMail.Body = strBody & "<P>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    objMail.Body &= objCommon.EmailFooter
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    objMail.Body &= objCommon.EmailFooterENT
                    'End If
                    'objMail.Body &= objCommon.EmailCompGen
                    objMail.Subject = "Agora : " & strSubject
                    objMail.SendMail()

                Case EmailType.CompanyActivated
                    '      If pEmailType = 32 Then
                    ' pPara1 = V - to All vendor company user; B - to All Buyer company user
                    strSubject = "Company Activated"
                    Dim strsqlCoy As String
                    Dim dsCoy As New DataSet
                    Dim objDb As New EAD.DBCom
                    Dim strCoyName As String = ConfigurationSettings.AppSettings("CompanyName")
                    Dim strSysName As String = ConfigurationSettings.AppSettings("SystemName")
                    blnAttn = True
                    'Zulham 22112018
                    strTo = ConfigurationSettings.AppSettings("HelpdeskEmail")
                    Select Case pPara1
                        Case "V"
                            objComp = objComps.GetCompanyDetails(strSCoyId)
                            strCond = " AND UM_STATUS = 'A' "
                            ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId, strCond)
                            strBody = "Dear Vendor, <BR>"
                            '                            strBody &= "<P>Please be informed that your company " & objComp.CoyName & " has been reactivated within TX123 system after had settled the following item(s): <BR>"
                            strBody &= "<P>Please be informed that your company " & objComp.CoyName & " has been reactivated within " & strSysName & " system after had settled the following item(s): <BR>" 'CR0061
                            strBody &= "<P>1. Renew the annual subscription package with " & strCoyName & "<BR>"
                            strBody &= "2. Continue the subscription package with " & strCoyName & "<BR>"
                            strBody &= "<P>Your user account login ID is now ready to login into " & strSysName & " system.<BR>"
                            strBody &= "<P>All transactions have been opened from accessing.<BR>"
                            strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
                            strBody &= "<P>Thank you. <BR>"
                            'zulham 22112018
                            'For i = 0 To ds.Tables("USERTO").Rows.Count - 1
                            '    If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" Then
                            '        strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","
                            '    End If
                            'Next

                            If strTo <> "" Then
                                objMail.MailTo = strTo
                                objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                'objMail.Body = strBody & "<P>"
                                'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                '    objMail.Body &= objCommon.EmailFooter
                                'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                '    objMail.Body &= objCommon.EmailFooterENT
                                'End If
                                'objMail.Body &= objCommon.EmailCompGen
                                objMail.Subject = "Agora : " & strSubject
                                objMail.SendMail()
                            End If

                        Case "B"
                            strCond = " AND UM_STATUS = 'A' "
                            strCond &= "AND UGM_FIXED_ROLE IN ('" & FixedRole.Buyer.ToString & "','" & Replace(FixedRole.Buyer_Administrator.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "') "
                            objComp = objComps.GetCompanyDetails(strSCoyId)

                            strsqlCoy = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR "
                            strsqlCoy &= "INNER JOIN COMPANY_MSTR ON CV_B_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
                            strsqlCoy &= "WHERE CV_S_COY_ID = '" & Common.Parse(strSCoyId) & "' "
                            dsCoy = objDb.FillDs(strsqlCoy) ' get all buyer company that deactivated vendor company is their approved vendor

                            For i = 0 To dsCoy.Tables(0).Rows.Count - 1
                                strTo = ""
                                ds = objUsers.getUserEmailList(FixedRole.Buyer, dsCoy.Tables(0).Rows(i)("CV_B_COY_ID"), strCond)

                                strBody = "Dear Buyer Company, <BR>"
                                strBody &= "<P>Please be informed that vendor company - " & objComp.CoyName & " has been reactivated within " & strSysName & " system after had settled the following item(s): <BR>"
                                strBody &= "<P>1. Renew the annual subscription package with " & strCoyName & "<BR>"
                                strBody &= "2. Continue the subscription package with " & strCoyName & "<BR>"
                                strBody &= "<P>All related transactions have been re-opened for accessing for " & objComp.CoyName & ".<BR>"
                                strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
                                strBody &= "<P>Thank you. <BR>"
                                'Zulham 23112018
                                strTo = ConfigurationSettings.AppSettings("HelpdeskEmail")
                                'Zulham 22112018
                                'For j = 0 To ds.Tables("USERTO").Rows.Count - 1
                                '    If Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) <> "" Then
                                '        strTo &= Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) & ","
                                '    End If
                                'Next

                                If strTo <> "" Then
                                    objMail.MailTo = strTo
                                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                    'objMail.Body = strBody & "<P>"
                                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                    '    objMail.Body &= objCommon.EmailFooter
                                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                    '    objMail.Body &= objCommon.EmailFooterENT
                                    'End If
                                    'objMail.Body &= objCommon.EmailCompGen
                                    objMail.Subject = "Agora : " & strSubject
                                    objMail.SendMail()
                                End If
                            Next
                    End Select
                    '       End If

                Case EmailType.CompanyDeactivated
                    ' pPara1 = V - to All vendor company user; B - to All Buyer company user
                    strSubject = "Company Deactivated"
                    Dim strsqlCoy As String
                    Dim dsCoy As New DataSet
                    Dim objDb As New EAD.DBCom
                    Dim strCoyName As String = ConfigurationSettings.AppSettings("CompanyName") 'CR0061
                    Dim strSysName As String = ConfigurationSettings.AppSettings("SystemName") 'CR0061
                    blnAttn = True

                    Select Case pPara1
                        Case "V"
                            objComp = objComps.GetCompanyDetails(strSCoyId)

                            ' cannot use existing getUserEmailList coz it needs to retrieve inactive company's users
                            strsql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS "
                            strsql &= "FROM USER_MSTR LEFT JOIN COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'I' AND CM_DELETED = 'N' "
                            strsql &= "RIGHT JOIN USERS_USRGRP ON UU_COY_ID = CM_COY_ID AND UU_USER_ID = UM_USER_ID "
                            strsql &= "LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID = UU_USRGRP_ID "
                            strsql &= "WHERE UM_COY_ID = '" & strSCoyId & "' AND UM_DELETED <> 'Y'  AND UM_STATUS = 'A' "
                            'ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId, strCond)
                            ds = objDb.FillDs(strsql)
                            ds.Tables(0).TableName = "USERTO"

                            strBody = "Dear Vendor, <BR>"
                            strBody &= "<P>Please be informed that your company " & objComp.CoyName & " has been deactivated from " & strSysName & " system for the following reason(s): <BR>"
                            strBody &= "<P>1. Did not renew the annual subscription package with " & strCoyName & "<BR>"
                            strBody &= "2. Wish to terminate the subscription package with " & strCoyName & "<BR>"
                            strBody &= "<P>Your user account login ID is no longer valid and cannot login into " & strSysName & " system.<BR>"
                            strBody &= "<P>All transactions have been locked from accessing.<BR>"
                            strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
                            strBody &= "<P>Thank you. <BR>"

                            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
                                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" Then
                                    strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","
                                End If
                            Next

                            If strTo <> "" Then
                                objMail.MailTo = strTo
                                objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                'objMail.Body = strBody & "<P>"
                                'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                '    objMail.Body &= objCommon.EmailFooter
                                'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                '    objMail.Body &= objCommon.EmailFooterENT
                                'End If
                                'objMail.Body &= objCommon.EmailCompGen
                                objMail.Subject = "Agora : " & strSubject
                                objMail.SendMail()
                            End If

                        Case "B"
                            strCond = " AND UM_STATUS = 'A' "
                            strCond &= "AND UGM_FIXED_ROLE IN ('" & FixedRole.Buyer.ToString & "','" & Replace(FixedRole.Buyer_Administrator.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "') "
                            objComp = objComps.GetCompanyDetails(strSCoyId)

                            strsqlCoy = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR "
                            strsqlCoy &= "INNER JOIN COMPANY_MSTR ON CV_B_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
                            strsqlCoy &= "WHERE CV_S_COY_ID = '" & Common.Parse(strSCoyId) & "' "
                            dsCoy = objDb.FillDs(strsqlCoy) ' get all buyer company that deactivated vendor company is their approved vendor

                            For i = 0 To dsCoy.Tables(0).Rows.Count - 1
                                strTo = ""
                                ds = objUsers.getUserEmailList(FixedRole.Buyer, dsCoy.Tables(0).Rows(i)("CV_B_COY_ID"), strCond)

                                strBody = "Dear Buyer Company, <BR>"
                                strBody &= "<P>Please be informed that vendor company - " & objComp.CoyName & " has been deactivated from " & strSysName & " system for the following reason(s): <BR>"
                                strBody &= "<P>1. Did not renew the annual subscription package with " & strCoyName & "<BR>"
                                strBody &= "2. Wish to terminate the subscription package with " & strCoyName & "<BR>"
                                strBody &= "<P>All transactions that related to " & objComp.CoyName & " can still be viewed within " & strSysName & " system but any further action performed onto those transactions will not be responded by the vendor company.<BR>"
                                strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
                                strBody &= "<P>Thank you. <BR>"

                                For j = 0 To ds.Tables("USERTO").Rows.Count - 1
                                    If Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) <> "" Then
                                        strTo &= Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) & ","
                                    End If
                                Next

                                If strTo <> "" Then
                                    objMail.MailTo = strTo
                                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                                    'objMail.Body = strBody & "<P>"
                                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                                    '    objMail.Body &= objCommon.EmailFooter
                                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                                    '    objMail.Body &= objCommon.EmailFooterENT
                                    'End If
                                    'objMail.Body &= objCommon.EmailCompGen
                                    objMail.Subject = "Agora : " & strSubject
                                    objMail.SendMail()
                                End If
                            Next
                    End Select

                Case EmailType.IQCApprovedToSK 'IQC Approved and sent to Storekeeper (Developed by Chee Hong - 15052013)
                    strSubject = "IQC Approved"
                    strCond = "AND UM_USER_ID = '" & strUserId & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)
                    'objComp = objComps.GetCompanyDetails(strSCoyId)

                    strRole = "Storekeeper"
                    strBody &= "<P>IQC (" & strDocNo & ") has been approved by IQC Officer.<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.IQCRejectedToSK 'IQC Rejecetd and sent to Storekeeper (Developed by Chee Hong - 15052013)
                    Select Case pPara1
                        Case "w"
                            strSubject = "IQC Rejected"
                            strCond = "AND UM_USER_ID = '" & strUserId & "' "
                            ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                            strRole = "Storekeeper"
                            strBody &= "<P>IQC (" & strDocNo & ") has been rejected with further waive action by IQC Officer.<BR>"
                            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        Case "r"
                            strSubject = "IQC Rejected"
                            strCond = "AND UM_USER_ID = '" & strUserId & "' "
                            ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                            strRole = "Storekeeper"
                            strBody &= "<P>IQC (" & strDocNo & ") has been rejected with further replacement action (" & pPara2 & ") by IQC Officer.<BR>"
                            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                        Case ""
                            strSubject = "IQC Rejected"
                            If pPara2 = "SK" Then
                                strCond = "AND UM_USER_ID = '" & strUserId & "' "
                                ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)
                                strRole = "Storekeeper"
                            ElseIf pPara2 = "FM" Then
                                ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId)
                                strRole = "Finance Manager"
                            ElseIf pPara2 = "AO" Then
                                strCond = "AND UM_USER_ID = '" & strUserId & "' "
                                ds = objUsers.getUserEmailList(FixedRole.Quality_Control_Officer, strBCoyId, strCond)
                                strRole = "IQC Officer"
                            End If

                            strBody &= "<P>IQC (" & strDocNo & ") has been rejected with no replacement action by IQC Officer.<BR>"
                            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    End Select

                Case EmailType.IRRejectedToRequestor
                    strSubject = "IR Rejected"
                    strCond = "AND UM_USER_ID = '" & strUserId & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    strRole = "Requestor"
                    strBody &= "<P>IR (" & strDocNo & ") has been rejected by your HOD (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.MRSIssued
                    strSubject = "MRS Issued/ Partial Issued"
                    strCond = "AND UM_USER_ID = '" & strUserId & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    If pPara2 = "" Then
                        strRole = "Requestor"
                    Else
                        strRole = pPara2
                    End If
                    strBody &= "<P>The MRS (" & strDocNo & ") has been issued/ partial issued by your Storekeeper (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.MRSRejected
                    strSubject = "MRS Rejected"
                    strCond = "AND UM_USER_ID = '" & strUserId & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    If pPara2 = "" Then
                        strRole = "Requestor"
                    Else
                        strRole = pPara2
                    End If
                    strBody &= "<P>The MRS (" & strDocNo & ") has been rejected by your Storekeeper (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.RIAcknowledged
                    strSubject = "Return Inward Acknowledged"
                    strCond = "AND UM_USER_ID = '" & pPara2 & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    strRole = "Requester"
                    strBody &= "<P>Return Inward (" & strDocNo & ") has been acknowledged by your Storekeeper (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.RIRejected
                    strSubject = "Return Inward Rejected"
                    strCond = "AND UM_USER_ID = '" & pPara2 & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    strRole = "Requester"
                    strBody &= "<P>Return Inward (" & strDocNo & ") has been rejected by your Storekeeper (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.IRToSK
                    strSubject = "IR Created"
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId)

                    strRole = "Storekeeper"
                    If pPara1 = "Y" Then
                        strBody &= "<P>The urgent Inventory Requisition (" & strDocNo & ") has been submitted by your Requester (" & pPara2 & ").<BR>"
                    Else
                        strBody &= "<P>The Inventory Requisition (" & strDocNo & ") has been submitted by your Requester (" & pPara2 & ").<BR>"
                    End If
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.MRSToSK
                    strSubject = "MRS Created"
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId)

                    strRole = "Storekeeper"
                    If pPara1 = "Y" Then
                        strBody &= "<P>You have an urgent MRS (" & strDocNo & ") waiting for your action.<BR>"
                    Else
                        strBody &= "<P>You have an MRS (" & strDocNo & ") waiting for your action.<BR>"
                    End If
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.ROCreated
                    strSubject = "Return Outward Created"
                    ds = objUsers.getUserEmailList("Invoice", FixedRole.Finance_Manager, strBCoyId)

                    strRole = "Finance Manager"
                    strBody &= "<P>The Return Outward (" & strDocNo & ") has been submitted by your Storekeeper (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.RICreated
                    strSubject = "Return Inward Created"
                    strCond = "AND UM_USER_ID = '" & strUserId & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Store_Keeper, strBCoyId, strCond)

                    If pPara1 = "sk" Then
                        strRole = "Storekeeper"
                    ElseIf pPara1 = "hod" Then
                        strRole = "HOD"
                    Else
                        strRole = ""
                    End If

                    strBody &= "<P>Return Inward (" & strDocNo & ") has been submitted by your Requestor (" & pPara2 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'Jules 2015.02.02 Agora Stage 2
                Case EmailType.FOIncomingDN ' Vendor to Buyer Finance Officer
                    strSubject = "Debit Note/Debit Advice Created"

                    'Jules 2019.02.07 - Only send email to the first FO as per invoice flow.
                    'ds = objUsers.getUserEmailList(FixedRole.Finance_Officer, strBCoyId)
                    Dim strSql1 As String
                    Dim strInvAppr As String = ""
                    Dim objDB As New EAD.DBCom

                    strSql1 = "Select CM_INV_APPR from COMPANY_MSTR where CM_COY_ID = '" & strBCoyId & "'"
                    Dim tDS As DataSet = objDB.FillDs(strSql1)

                    If tDS.Tables(0).Rows.Count > 0 Then
                        strInvAppr = tDS.Tables(0).Rows(0).Item("CM_INV_APPR")
                    End If

                    Dim strSQL2, strInvNo As String
                    strSQL2 = "SELECT DNM_INV_NO FROM DEBIT_NOTE_MSTR WHERE DNM_DN_NO='" & strDocNo & "'"
                    Dim dsInv As DataSet = objDB.FillDs(strSQL2)
                    If dsInv.Tables(0).Rows.Count > 0 Then
                        strInvNo = dsInv.Tables(0).Rows(0).Item("DNM_INV_NO")
                    End If

                    If strInvAppr = "Y" Then
                        ds = objUsers.getFirstFOEmailList(strBCoyId, strInvNo, strSCoyId)
                    Else
                        ds = objUsers.getUserEmailList(FixedRole.Finance_Officer, strBCoyId)
                    End If
                    'End modification.

                    strRole = "Finance Officer"
                    'strBody = "Dear " & ds.Tables("USERTO").Rows(0).Item("UM_USER_NAME") & "," 'Agora GSt Stage 2 - CH - 11 Feb 2015
                    strBody &= "<P>You have a Debit Note/Debit Advice (" & strDocNo & ") waiting for your action. <BR>"

                Case EmailType.FMIncomingDN ' Buyer - Finance Officer to Finance Manager
                    strSubject = "Debit Note/Debit Advice Verified"
                    ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId)

                    strRole = "Finance Manager"
                    'strBody = "Dear " & ds.Tables("USERTO").Rows(0).Item("UM_USER_NAME") & "," 'Agora GSt Stage 2 - CH - 11 Feb 2015
                    strBody &= "<P>You have a Debit Note/Debit Advice (" & strDocNo & ") waiting for your action. <BR>"

                Case EmailType.FMIncomingCN ' Vendor to Buyer Finance Manager
                    strSubject = "Credit Note/Credit Advice Created"
                    'ds = objUsers.getFirstFOEmailList(strBCoyId, strDocNo2)
                    'Issue 7480 - CH - 23 Mar 2015 (No.36)

                    'Jules 2019.02.15 - Send email only to the FM involved in the workflow.
                    'ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId)
                    strCond = "AND UM_USER_ID IN (SELECT FA_AO FROM CREDIT_NOTE_MSTR INNER JOIN INVOICE_MSTR ON CNM_CN_S_COY_ID=IM_S_COY_ID " &
                            "INNER JOIN FINANCE_APPROVAL ON FA_INVOICE_INDEX=IM_INVOICE_INDEX " &
                            "WHERE CNM_CN_B_COY_ID = '" & strBCoyId & "' AND CNM_CN_STATUS IN ('1') AND CNM_CN_NO='" & strDocNo & "' AND FA_AGA_TYPE='FM')"
                    ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, strBCoyId, strCond)
                    'End modification.



                    strRole = "Finance Manager"
                    'strBody = "Dear " & ds.Tables("USERTO").Rows(0).Item("UM_USER_NAME") & "," 'Agora GSt Stage 2 - CH - 11 Feb 2015
                    strBody &= "<P>You have a Credit Note/Credit Advice (" & strDocNo & ") waiting for your action. <BR>"

                Case EmailType.SCApproved
                    strSubject = "Staff Claim Approved"
                    strCond = "AND UM_USER_ID = '" & pPara2 & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)

                    strRole = "Staff"
                    strBody &= "<P>Your Staff Claim (" & strDocNo & ") has been approved by your Approving Officer (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                Case EmailType.SCRejected
                    strSubject = "Staff Claim Rejected"
                    strCond = "AND UM_USER_ID = '" & pPara2 & "' "
                    ds = objUsers.getUserEmailList(FixedRole.Approving_Officer, strBCoyId, strCond)

                    strRole = "Staff"
                    strBody &= "<P>Your Staff Claim (" & strDocNo & ") has been rejected by your Approving Officer (" & pPara1 & ").<BR>"
                    strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

            End Select
            ''Michelle (13/4/2009) - To force the system to process the Company Activation as the system will somehow process the NewUserAccount eventhough
            ''                       the type is Company Activated
            'If pEmailType = 32 Then
            '    ' pPara1 = V - to All vendor company user; B - to All Buyer company user
            '    strSubject = "Company Activated"
            '    Dim strsqlCoy As String
            '    Dim dsCoy As New DataSet
            '    Dim objDb As New EAD.DBCom
            '    blnAttn = True

            '    Select Case pPara1
            '        Case "V"
            '            objComp = objComps.GetCompanyDetails(strSCoyId)
            '            strCond = " AND UM_STATUS = 'A' "
            '            ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId, strCond)
            '            strBody = "Dear Vendor, <BR>"
            '            strBody &= "<P>Please be informed that your company " & objComp.CoyName & " has been reactivated within TX123 system after had settled the following item(s): <BR>"
            '            strBody &= "<P>1. Renew the annual subscription package with TX123<BR>"
            '            strBody &= "2. Continue the subscription package with TX123<BR>"
            '            strBody &= "<P>Your user account login ID is now ready to login into TX123 system.<BR>"
            '            strBody &= "<P>All transactions have been opened from accessing.<BR>"
            '            strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
            '            strBody &= "<P>Thank you. <BR>"

            '            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
            '                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" Then
            '                    strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","
            '                End If
            '            Next

            '            If strTo <> "" Then
            '                objMail.MailTo = strTo
            '                objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            '                objMail.Subject = "eProcure : " & strSubject
            '                objMail.SendMail()
            '            End If

            '        Case "B"
            '            strCond = " AND UM_STATUS = 'A' "
            '            strCond &= "AND UGM_FIXED_ROLE IN ('" & FixedRole.Buyer.ToString & "','" & Replace(FixedRole.Buyer_Administrator.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "') "
            '            objComp = objComps.GetCompanyDetails(strSCoyId)

            '            strsqlCoy = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR "
            '            strsqlCoy &= "INNER JOIN COMPANY_MSTR ON CV_B_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
            '            strsqlCoy &= "WHERE CV_S_COY_ID = '" & Common.Parse(strSCoyId) & "' "
            '            dsCoy = objDb.FillDs(strsqlCoy) ' get all buyer company that deactivated vendor company is their approved vendor

            '            For i = 0 To dsCoy.Tables(0).Rows.Count - 1
            '                strTo = ""
            '                ds = objUsers.getUserEmailList(FixedRole.Buyer, dsCoy.Tables(0).Rows(i)("CV_B_COY_ID"), strCond)

            '                strBody = "Dear Buyer Company, <BR>"
            '                strBody &= "<P>Please be informed that vendor company - " & objComp.CoyName & " has been reactivated within TX123 system after had settled the following item(s): <BR>"
            '                strBody &= "<P>1. Renew the annual subscription package with TX123<BR>"
            '                strBody &= "2. Continue the subscription package with TX123<BR>"
            '                strBody &= "<P>All related transactions have been re-opened for accessing for " & objComp.CoyName & ".<BR>"
            '                strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
            '                strBody &= "<P>Thank you. <BR>"

            '                For j = 0 To ds.Tables("USERTO").Rows.Count - 1
            '                    If Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) <> "" Then
            '                        strTo &= Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) & ","
            '                    End If
            '                Next

            '                If strTo <> "" Then
            '                    objMail.MailTo = strTo
            '                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            '                    objMail.Subject = "eProcure : " & strSubject
            '                    objMail.SendMail()
            '                End If
            '            Next
            '    End Select

            '    'Michelle (13/4/2009) - To force the system to process the Company Deactivation as the system will somehow process the Company Activation eventhough
            '    '                       the type is Company Deactivated
            'ElseIf pEmailType = 33 Then  'EmailType.CompanyDeactivated
            '    ' pPara1 = V - to All vendor company user; B - to All Buyer company user
            '    strSubject = "Company Deactivated"
            '    Dim strsqlCoy As String
            '    Dim dsCoy As New DataSet
            '    Dim objDb As New EAD.DBCom
            '    blnAttn = True

            '    Select Case pPara1
            '        Case "V"
            '            objComp = objComps.GetCompanyDetails(strSCoyId)

            '            ' cannot use existing getUserEmailList coz it needs to retrieve inactive company's users
            '            strsql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS "
            '            strsql &= "FROM USER_MSTR LEFT JOIN COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'I' AND CM_DELETED = 'N' "
            '            strsql &= "RIGHT JOIN USERS_USRGRP ON UU_COY_ID = CM_COY_ID AND UU_USER_ID = UM_USER_ID "
            '            strsql &= "LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID = UU_USRGRP_ID "
            '            strsql &= "WHERE UM_COY_ID = '" & strSCoyId & "' AND UM_DELETED <> 'Y'  AND UM_STATUS = 'A' "
            '            'ds = objUsers.getUserEmailList(FixedRole.Vendor, strSCoyId, strCond)
            '            ds = objdb.FillDs(strsql)
            '            ds.Tables(0).TableName = "USERTO"

            '            strBody = "Dear Vendor, <BR>"
            '            strBody &= "<P>Please be informed that your company " & objComp.CoyName & " has been deactivated from TX123 system for the following reason(s): <BR>"
            '            strBody &= "<P>1. Did not renew the annual subscription package with TX123<BR>"
            '            strBody &= "2. Wish to terminate the subscription package with TX123<BR>"
            '            strBody &= "<P>Your user account login ID is no longer valid and cannot login into TX123 system.<BR>"
            '            strBody &= "<P>All transactions have been locked from accessing.<BR>"
            '            strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
            '            strBody &= "<P>Thank you. <BR>"

            '            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
            '                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" Then
            '                    strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","
            '                End If
            '            Next

            '            If strTo <> "" Then
            '                objMail.MailTo = strTo
            '                objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            '                objMail.Subject = "eProcure : " & strSubject
            '                objMail.SendMail()
            '            End If

            '        Case "B"
            '            strCond = " AND UM_STATUS = 'A' "
            '            strCond &= "AND UGM_FIXED_ROLE IN ('" & FixedRole.Buyer.ToString & "','" & Replace(FixedRole.Buyer_Administrator.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "') "
            '            objComp = objComps.GetCompanyDetails(strSCoyId)

            '            strsqlCoy = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR "
            '            strsqlCoy &= "INNER JOIN COMPANY_MSTR ON CV_B_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
            '            strsqlCoy &= "WHERE CV_S_COY_ID = '" & Common.Parse(strSCoyId) & "' "
            '            dsCoy = objDb.FillDs(strsqlCoy) ' get all buyer company that deactivated vendor company is their approved vendor

            '            For i = 0 To dsCoy.Tables(0).Rows.Count - 1
            '                strTo = ""
            '                ds = objUsers.getUserEmailList(FixedRole.Buyer, dsCoy.Tables(0).Rows(i)("CV_B_COY_ID"), strCond)

            '                strBody = "Dear Buyer Company, <BR>"
            '                strBody &= "<P>Please be informed that vendor company - " & objComp.CoyName & " has been deactivated from TX123 system for the following reason(s): <BR>"
            '                strBody &= "<P>1. Did not renew the annual subscription package with TX123<BR>"
            '                strBody &= "2. Wish to terminate the subscription package with TX123<BR>"
            '                strBody &= "<P>All transactions that related to " & objComp.CoyName & " can still be viewed within TX123 system but any further action performed onto those transactions will not be responded by the vendor company.<BR>"
            '                strBody &= "<P>If you have further enquiries, please call our helpdesk at " & ConfigurationSettings.AppSettings("AdminPhone") & ".<BR>"
            '                strBody &= "<P>Thank you. <BR>"

            '                For j = 0 To ds.Tables("USERTO").Rows.Count - 1
            '                    If Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) <> "" Then
            '                        strTo &= Common.parseNull(ds.Tables("USERTO").Rows(j)("UM_EMAIL")) & ","
            '                    End If
            '                Next

            '                If strTo <> "" Then
            '                    objMail.MailTo = strTo
            '                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            '                    objMail.Subject = "eProcure : " & strSubject
            '                    objMail.SendMail()
            '                End If
            '            Next
            '    End Select
            'End If

            If Not blnAttn Then  ' email already been sent above
                SendMailByDS(ds, strBody, strSubject, strRole)
            End If


            '            If Not ds Is Nothing Then
            '                'Table 0/dt(0) will send as TO
            '                If ds.Tables.Count > 0 Then
            '                    If ds.Tables("COMPANY").Rows.Count > 0 Then
            '                        If ds.Tables("COMPANY").Rows(0)("CM_DELETED") <> "Y" And ds.Tables("COMPANY").Rows(0)("CM_STATUS") = "A" Then
            '                            For i = 0 To ds.Tables("USERTO").Rows.Count - 1
            '                                If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
            '                                    blnAttn = True
            '                                    strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & "; "
            '                                Else
            '                                    dtr = dtAttn.NewRow()
            '                                    dtr("UserId") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_ID"))
            '                                    dtr("UserName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME"))
            '                                    dtr("Email") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
            '                                    dtr("CoyName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("CM_COY_NAME"))
            '                                    dtAttn.Rows.Add(dtr)
            '                                End If
            '                            Next

            '                            'kk.291204.Remark.Table 1/dt(1) will send as CC, blnAttn true to ensure have to otherwise will possible no TO but CC
            '                            'to smbody
            '                            If ds.Tables.Count = 3 And blnAttn Then
            '                                For i = 0 To ds.Tables("USERCC").Rows.Count - 1
            '                                    If Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
            '                                        strCC &= Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL")) & "; "
            '                                    Else
            '                                        dtr = dtAttn.NewRow()
            '                                        dtr("UserId") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_ID"))
            '                                        dtr("UserName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_USER_NAME"))
            '                                        dtr("Email") = Common.parseNull(ds.Tables("USERCC").Rows(i)("UM_EMAIL"))
            '                                        dtr("CoyName") = Common.parseNull(ds.Tables("USERCC").Rows(i)("CM_COY_NAME"))
            '                                        dtAttn.Rows.Add(dtr)
            '                                    End If
            '                                Next
            '                            End If
            '                        Else ' company not active
            '                            strEmailType = "4"
            '                            GoTo GetEmailContent
            '                        End If
            '                    End If ' ds.Tables("COMPANY").Rows.Count > 0
            '                End If ' ds.Tables.Count > 0
            '            End If ' ds is nothing

            '            If blnAttn Then
            '                strEmailType = "1"
            '            Else
            '                If dtAttn.Rows.Count = 0 Then
            '                    strEmailType = "3"
            '                Else
            '                    strEmailType = "2"
            '                End If
            '            End If

            'GetEmailContent:
            '            strLine = Strings.StrDup(objCommon.EmailCompGen.Length, ".")
            '            Select Case strEmailType
            '                Case "1"
            '                    objMail.MailTo = strTo
            '                    objMail.MailCc = strCC
            '                    objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            '                    objMail.Subject = "eProcure : " & strSubject

            '                Case "2"
            '                    objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")  ' EmailHub
            '                    strExtra = "Dear Hub Admin, <BR>"
            '                    For i = 0 To dtAttn.Rows.Count - 1
            '                        strExtra &= "User account for " & dtAttn.Rows(i)("UserId") & " (" & dtAttn.Rows(i)("UserId") & ", " & dtAttn.Rows(i)("Email") & ") of " & dtAttn.Rows(i)("CoyName") & " company has been deactivated. <BR>"
            '                    Next

            '                    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
            '                    objMail.Subject = "eProcure : Deactivated User - " & strSubject

            '                Case "3"
            '                    strExtra = "Dear Hub Admin, <BR>"
            '                    strExtra &= "User not found from " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & ". <BR>"
            '                    objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") 'EmailHub 

            '                    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
            '                    objMail.Subject = "eProcure : No User Exists - " & strSubject

            '                Case "4"
            '                    strExtra = "Dear Hub Admin, <BR>"
            '                    strExtra &= "<P>The company " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & " has been deactived. <BR> "
            '                    objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin") ' EmailHub 

            '                    objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen & "<BR><P>" & strLine
            '                    objMail.Subject = "eProcure : Deactivated Company - " & strSubject
            '            End Select

            'objMail.SendMail()
            objUser = Nothing
            objUsers = Nothing
            objComp = Nothing
            objComps = Nothing
        End Function

        Public Function sendItemUpdateMail(ByVal strSCoyId As String, ByVal strGroupIndex As String, ByVal dt1 As DataTable, ByVal dt2 As DataTable, ByVal blnList As Boolean, ByVal blnDiscount As Boolean)
            ' strGroupIndex - Discount Group Code List
            ' dt1 - New Catalogue Item added
            ' dt2 - Existing Catalogue Item Updated
            ' blnList = True id List Price has been modified
            ' blnDiscount = True id Discount Price has been modified

            Dim strSubject, strBody, strTo As String
            Dim strDear As String
            Dim strDetails As String
            Dim objComp As New Company
            Dim objComps As New Companies
            Dim objUser As New User
            Dim objUsers As New Users
            Dim objMail As New AppMail
            Dim ds As New DataSet
            Dim i, intType As Integer
            Dim objCommon As New Common
            Dim dtAttn As New DataTable
            dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
            dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
            dtAttn.Columns.Add("Email", Type.GetType("System.String"))
            dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
            Dim dtr As DataRow

            strSubject = "Vendor Catalogue Item Update"
            objComp = objComps.GetCompanyDetails(strSCoyId)

            If blnList And blnDiscount Then
                intType = 0
            ElseIf blnList Then
                intType = 1
            ElseIf blnDiscount Then
                intType = 2
            End If

            ds = objUsers.getPMEmailList(FixedRole.Purchasing_Manager, strSCoyId, intType, strGroupIndex)

            strBody = "<P>Vendor Company " & objComp.CoyName & " has added/updated the following catalogue item to their online catalogue: <BR>"
            If dt1.Rows.Count > 0 Then
                strBody &= "<P><U>New Catalogue Item Added:</U>"
                strBody &= "<TABLE WIDTH='70%'>"
                strBody &= "<TR><TD ALIGN='left' width='20%'><U>Item ID</U></TD><TD ALIGN='left' WIDTH='80%'><U>Item Description</U></TD></TR>"
                For i = 0 To dt1.Rows.Count - 1
                    strBody &= "<TR>"
                    strBody &= "<TD valign='top'>" & dt1.Rows(i)("ItemId") & "</TD><TD ALIGN='left' valign='top'>" & dt1.Rows(i)("ItemDesc") & "</TD>"
                    strBody &= "</TR>"
                Next
                strBody &= "</TABLE>"
            End If

            If dt2.Rows.Count > 0 Then
                strBody &= "<P><U>Existing Catalogue Item Updated:</U>"
                strBody &= "<TABLE WIDTH='70%'>"
                strBody &= "<TR><TD ALIGN='left' width='20%' ><U>Item ID</U></TD><TD ALIGN='left' WIDTH='50%'><U>Item Description</U></TD><TD ALIGN='left' WIDTH='15%' nowrap><U>List Price</U></TD><TD ALIGN='left' WIDTH='15%' nowrap><U>Discount Price</U></TD></TR>"
                For i = 0 To dt2.Rows.Count - 1
                    strBody &= "<TR>"
                    strBody &= "<TD  valign='top'>" & dt2.Rows(i)("ItemId") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("ItemDesc") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("List") & "</TD><TD ALIGN='left' valign='top'>" & dt2.Rows(i)("Discount") & "</TD>"
                    strBody &= "</TR>"
                Next
                strBody &= "</TABLE>"
            End If

            If Not ds Is Nothing Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    strTo = Common.parseNull(ds.Tables(0).Rows(i)("UM_EMAIL"))

                    strDear = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Purchasing Manager), <BR>"
                    'strBody = "Dear " & Common.parseNull(ds.Tables(0).Rows(i)("UM_USER_NAME")) & " (Purchasing Manager), <BR>" & strBody
                    'strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    'strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    '    strDetails = "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    'strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
                    '    strDetails = "<P>For more details, please login to " & objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"

                    'End If
                    strDetails = "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
                    'objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
                    'objMail.Body = strBody & "<P>"
                    objMail.Body = strDear & strBody & strDetails & "<P>"
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    objMail.Body &= objCommon.EmailFooter
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    objMail.Body &= objCommon.EmailFooterENT
                    'End If
                    objMail.Body &= objCommon.EmailFooter
                    objMail.Body &= objCommon.EmailCompGen
                    objMail.MailTo = strTo
                    objMail.Subject = "Agora : " & strSubject
                    objMail.SendMail()
                Next
            End If ' ds is nothing

            objUser = Nothing
            objUsers = Nothing
            objComp = Nothing
            objComps = Nothing
        End Function
        Public Function sendNewUserAccBilling(ByVal pEmailType As EmailType, ByVal strUserId As String,
                                            ByVal strBCoyId As String, ByVal strSCoyId As String,
                                            ByVal strDocNo As String, ByVal strDocNo2 As String,
                                            Optional ByVal pPara1 As String = "",
                                            Optional ByVal pPara2 As String = "",
                                            Optional ByVal pPara3 As String = "")
            Dim strsql As String
            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim strBody, strCC, strTo, strSubject As String
            Dim objUser As New User
            Dim objUsers As New Users
            Dim i, j As Integer
            Dim blnAttn As Boolean = False
            Dim objComp As New Company
            Dim objComps As New Companies
            Dim objMail As New AppMail
            Dim strCond As String
            Dim strExtra As String = ""
            Dim strLine As String
            Dim strEmailType As String
            Dim strRole As String
            Dim objCommon As New Common
            '       If pEmailType = 33 Then
            strSubject = "New User Account Creation"
            objUser = objUsers.GetUserDetails(strUserId, strBCoyId)
            objComp = objComps.GetCompanyDetails(strBCoyId)
            blnAttn = True
            ds = objUsers.getUserEmailList(strUserId, strBCoyId)

            strBody = "Dear " & objUser.Name & ", <BR>"
            strBody &= "<P>Your user account has been created by your Admin.<BR>"
            strBody &= "<P>To access the application, Please use the following login information.<BR><P>"

            strBody &= "Company Name               : " & objComp.CoyName
            strBody &= "<BR>Company ID             : " & objComp.CoyId
            strBody &= "<BR>User ID                : " & objUser.UserID
            strBody &= "<BR>New Temporary Password : " & pPara1
            strBody &= "<BR>Expiry Date            : " & objUser.NextExpiredDt & "<BR><P>"

            If pPara3 <> "" Then
                strBody &= "<P>To access, please login to " & pPara3 & "<BR>"
            Else
                'strBody &= "<P>To access, please login to " & objCommon.EmailHomeEhubAddr & "<BR>"
                strBody &= "<P>To access, please login to " & objCommon.EmailHomeEhubAddr & "<BR>"
            End If

            objMail.MailTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
            'objMail.Body = strBody & "<P>" & objCommon.EmailFooter & objCommon.EmailCompGen
            objMail.Body = strBody & "<P>"
            objMail.Body &= objCommon.EmailFooter
            objMail.Body &= objCommon.EmailCompGen
            objMail.Subject = "Agora : " & strSubject
            objMail.SendMail()

        End Function
    End Class
End Namespace

