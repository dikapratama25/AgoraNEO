Imports System
Imports System.Collections
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Mail
Imports System.Configuration
Imports AgoraNEO
Imports AppGlobals
Imports System.Security.Cryptography
Imports System.Text

Namespace AgoraNEO
    '//To be continue.....
    Public Class User
        Public UserID As String
        Public Name As String
        Public CompanyID As String
        Public DeptID As String
        Public DeptName As String
        Public Password As String
        'Public PwdExpDate As Date
        Public Email As String
        Public PhoneNo As String
        Public FaxNo As String
        Public ApproveLimit As String
        Public POApproveLimit As String
        Public InvoiceApproveLimit As String
        Public Status As Char
        Public DeleteInd As String
        Public PwdLastChg As String
        Public NewPwdInd As Char
        Public LastLogin As Date
        Public NextExpiredDt As Date
        Public Question As Integer
        Public Answer As String
        Public UserGroup As String
        Public UserGroupName As String
        Public Designation As String
        Public MassApp As Char
        Public InvoiceMassApp As Char
        Public MrsMassApp As Char
        Public StockTypeSpot As Char
        Public StockTypeStock As Char
        Public StockTypeMro As Char
        Public PageCount As Integer
        Public CCList As String
        Public strFixedRole As String
        Public PolicyAgreeDt As String
        Public SCEmailOnOff As String
    End Class

    Public Class UserGroup
        Public Id As String
        Public Name As String
        Public FixedRole As String
        Public Type As String
        Public Delete As String
        Public Package As String
    End Class

    Public Class UserUsrGrp
        Public UserDetail As User
        Public CompanyId As String
        Protected Friend PackageList As ArrayList
        Protected Friend UserGroupList As ArrayList

        Public Sub New()
            UserDetail = New User
            PackageList = New ArrayList
            UserGroupList = New ArrayList
        End Sub

        Public Function PackageListCount() As Integer
            Return PackageList.Count
        End Function

        Public Function UserGroupListCount() As Integer
            Return UserGroupList.Count
        End Function

        Public Function Package(ByVal pIndex As Integer) As String
            Return PackageList.Item(pIndex)
        End Function

        Public Function UserGroup(ByVal pIndex As Integer) As String
            Return UserGroupList.Item(pIndex)
        End Function

        Public Sub AddPackageUserGroup(ByVal pPackageId As String, ByVal pUserGroupId As String)
            PackageList.Add(pPackageId)
            UserGroupList.Add(pUserGroupId)
        End Sub
    End Class

    Public Class Users
        Dim objDb As New EAD.DBCom   '(Configuration.ConfigurationSettings.AppSettings("SSO"))
        Dim lsSql As String
        Dim strUserMassage As String
        'Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Property Message() As String
            Get
                Message = strUserMassage
            End Get
            Set(ByVal Value As String)
                strUserMassage = Value
            End Set
        End Property
        Public Function getCompanyID(ByVal userID As String) As String
            Dim companyID As String
            companyID = objDb.GetVal("SELECT UM_COY_ID FROM USER_MSTR WHERE UM_USER_ID ='" & userID & "'")
            Return companyID
        End Function

        Private Function PopulateUser(ByVal dr As DataRow) As User
            Dim objUser As New User
            Dim cols As DataColumnCollection
            cols = dr.Table.Columns
            If cols.Contains("UM_USER_NAME") Then
                objUser.Name = Common.parseNull(dr("UM_USER_NAME"))
            End If
            If cols.Contains("UM_PHONE_NO") Then
                objUser.PhoneNo = Common.parseNull(dr("UM_PHONE_NO"))
            End If
            '//to be continued
            Return objUser
            objUser = Nothing
        End Function

        'Public Function getUserGrpByEmployee(ByVal pName As String, ByVal strColor As String, ByVal strApp As String) As String
        '    Dim strsql As String
        '    strsql = "SELECT *, '<font color=" & strColor & ">' as color1, '</font>' as color2 from USERS_USRGRP LEFT JOIN USER_GROUP_MSTR ON UU_USRGRP_ID=UGM_USRGRP_ID "
        '    strsql &= "AND UGM_APP_PKG = UU_APP_PKG "
        '    strsql &= "WHERE UU_USER_ID='" & Common.Parse(pName) & "' "
        '    strsql &= "AND UU_COY_ID='" & ctx.Session("CompanyId") & "' "
        '    strsql &= "AND UU_APP_PKG = '" & strApp & "' "
        '    Return strsql
        'End Function

        'Public Function getUserGrpByEmployee(ByVal pName As String, ByVal dvwPackage As DataView, Optional ByVal blnEnterpriseVersion As Boolean = True) As DataView
        '    Dim strSQl As String
        '    Dim i As Integer
        '    Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("SSO"))

        '    'If Not HttpContext.Current.Session("Env") = "FTN" Then
        '    If blnEnterpriseVersion = True Then
        '        If dvwPackage.Count = 1 Then
        '            strSQl &= getUserGrpByEmployee(pName, "Black", dvwPackage.Item(0)("AP_APP_ID"))
        '        Else
        '            For i = 0 To dvwPackage.Count - 1
        '                If i = dvwPackage.Count - 1 Then
        '                    strSQl &= getUserGrpByEmployee(pName, dvwPackage.Item(i)("AP_COLOR"), dvwPackage.Item(i)("AP_APP_ID"))
        '                Else
        '                    strSQl &= getUserGrpByEmployee(pName, dvwPackage.Item(i)("AP_COLOR"), dvwPackage.Item(i)("AP_APP_ID")) & " UNION "
        '                End If
        '            Next
        '        End If
        '    Else
        '        Dim dv As DataView
        '        dv = GetUserGroup(pName, getCompanyType(), False)
        '        If Not dv Is Nothing AndAlso dv.Count() > 0 Then
        '            For i = 0 To dv.Count() - 1
        '                If dv.Item(i).Item(1) = "FTN (PO)" Then
        '                    strSQl = "SELECT 'Purchasing Officer' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                    Exit For
        '                ElseIf dv.Item(i).Item(1) = "FTN (PM)" Then
        '                    strSQl = "SELECT 'Purchasing Manager' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                    Exit For
        '                ElseIf dv.Item(i).Item(1) = "FTN (AO)" Then
        '                    strSQl = "SELECT 'Approval Officer' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                    Exit For
        '                ElseIf dv.Item(i).Item(1) = "FTN (SK)" Then
        '                    strSQl = "SELECT 'Storekeeper' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                    Exit For
        '                ElseIf dv.Item(i).Item(1) = "FTN (Buyer)" Then
        '                    strSQl = "SELECT 'Buyer' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                    Exit For
        '                Else
        '                    strSQl = "SELECT '' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '                End If
        '            Next
        '        Else
        '            strSQl = "SELECT '' as UGM_USRGRP_NAME, '<font color=" & dvwPackage.Item(0)("AP_COLOR") & ">' as color1, '</font>' as color2"
        '        End If
        '    End If

        '    Return objDb.GetView(strSQl)
        '    objDb = Nothing
        'End Function

        Function getMd5Hash(ByVal input As String) As String
            ' Create a new instance of the MD5 object.
            Dim md5Hasher As MD5 = MD5.Create()

            ' Convert the input string to a byte array and compute the hash.
            Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))

            ' Create a new Stringbuilder to collect the bytes

            ' and create a string.

            Dim sBuilder As New StringBuilder()
            ' Loop through each byte of the hashed data

            ' and format each one as a hexadecimal string.

            Dim i As Integer

            For i = 0 To data.Length - 1

                sBuilder.Append(data(i).ToString("x2"))

            Next i
            ' Return the hexadecimal string.
            Return sBuilder.ToString()
        End Function
        Public Function GetUserPassword(ByVal strUserID As String, ByVal strCoyID As String, ByVal md5key As String, ByVal todaydate As String) As String
            Dim strpassword As String
            Dim userid As String
            userid = objDb.Get1Column("USER_MSTR", "UM_USER_ID", " WHERE UM_USER_ID='" & strUserID & "' AND UM_COY_ID='" & strCoyID & "'")
            userid &= todaydate
            Dim md5userid As String = getMd5Hash(UCase(userid))

            If md5userid = md5key Then
                strpassword = objDb.Get1Column("USER_MSTR", "CAST(UM_PASSWORD AS VARCHAR(100)) AS UM_PASSWORD", " WHERE UM_USER_ID='" & strUserID & "' AND UM_COY_ID='" & strCoyID & "'")
                Return strpassword
            Else
                Return Nothing
            End If
        End Function

        Public Function GetUserDetail(ByVal strUserID As String, ByVal strCoyID As String, ByRef objUser As User) As Boolean
            Dim lsSql As String
            Dim objDb As New EAD.DBCom 'mimi 20180711 : pamb

            lsSql = "SELECT * FROM USER_MSTR WHERE UM_USER_ID='" &
            strUserID & "' AND UM_COY_ID='" & strCoyID & "'" 'AND UM_STATUS<>'N'"
            Try
                Dim tDS As DataSet = objDb.FillDs(lsSql)
                If tDS.Tables(0).Rows.Count > 0 Then
                    objUser.Name = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_USER_NAME"))
                    objUser.PhoneNo = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_TEL_NO"))
                    objUser.FaxNo = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_FAX_NO"))
                    objUser.Email = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_EMAIL"))
                    GetUserDetail = True
                Else
                    GetUserDetail = False
                End If
            Catch ErrExp As Exception
                Common.TrwExp(ErrExp)
            Finally
                objDb = Nothing
            End Try

        End Function

        'Name       : GetUserDetails
        'Author     : kk
        'Descption  : Get all the User Particular 
        'Remark     : 
        'ReturnValue: UserDetail class
        'LastUpadte : 19 Sep 2002
        'Version    : 1.00
        Public Function GetUserDetails(ByVal pUserID As String, ByVal pCompID As String) As User
            Dim strSQL As String
            Dim tDS As DataSet
            Dim objDcom As New EAD.DBCom

            strSQL = " SELECT *, IFNULL(UM_PO_APP_LIMIT,0.00) AS UM_PO_APP,  CAST(UM_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_MSTR LEFT JOIN COMPANY_DEPT_MSTR ON UM_DEPT_ID = CDM_DEPT_CODE WHERE UM_USER_ID='" & Common.Parse(pUserID) & "' AND UM_COY_ID='" & Common.Parse(pCompID) & "'  AND (UM_DELETED = 'N' OR UM_DELETED = 'S' ) "
            Dim dt As DataTable = objDcom.FillDt(strSQL)
            Dim objUserDetail As New User
            If dt.Rows.Count > 0 Then
                objUserDetail.UserID = CStr(dt.Rows(0).Item("UM_USER_ID"))
                objUserDetail.DeleteInd = CStr(dt.Rows(0).Item("UM_DELETED"))
                'objUserDetail.Password = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_PASSWORD")), "", dt.Rows(0).Item("UM_PASSWORD")))
                objUserDetail.Password = CStr(IIf(IsDBNull(dt.Rows(0).Item("PWD")), "", dt.Rows(0).Item("PWD")))
                objUserDetail.Name = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_USER_NAME")), "", dt.Rows(0).Item("UM_USER_NAME")))
                objUserDetail.CompanyID = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_COY_ID")), "", dt.Rows(0).Item("UM_COY_ID")))
                objUserDetail.DeptID = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_DEPT_ID")), "", dt.Rows(0).Item("UM_DEPT_ID")))
                objUserDetail.DeptName = CStr(IIf(IsDBNull(dt.Rows(0).Item("CDM_DEPT_NAME")), "", dt.Rows(0).Item("CDM_DEPT_NAME")))
                objUserDetail.Email = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_EMAIL")), "", dt.Rows(0).Item("UM_EMAIL")))
                'Michelle (20/9/2010) - To eliminate the deciamal points
                objUserDetail.ApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_APP_LIMIT")), "", dt.Rows(0).Item("UM_APP_LIMIT")))
                objUserDetail.InvoiceApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")), "", dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")))
                objUserDetail.ApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_APP_LIMIT")), "", Val(dt.Rows(0).Item("UM_APP_LIMIT"))))
                objUserDetail.POApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_PO_APP")), "", Val(dt.Rows(0).Item("UM_PO_APP"))))
                objUserDetail.InvoiceApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")), "", Val(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT"))))
                objUserDetail.MassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_MASS_APP")), "N", dt.Rows(0).Item("UM_MASS_APP")))
                objUserDetail.InvoiceMassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_MASS_APP")), "N", dt.Rows(0).Item("UM_INVOICE_MASS_APP")))
                objUserDetail.MrsMassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_MRS_MASS_APP")), "N", dt.Rows(0).Item("UM_MRS_MASS_APP")))
                objUserDetail.Designation = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_DESIGNATION")), "", dt.Rows(0).Item("UM_DESIGNATION")))
                objUserDetail.PhoneNo = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_TEL_NO")), "", dt.Rows(0).Item("UM_TEL_NO")))
                objUserDetail.FaxNo = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_FAX_NO")), "", dt.Rows(0).Item("UM_FAX_NO")))
                objUserDetail.NewPwdInd = CStr(dt.Rows(0).Item("UM_NEW_PASSWORD_IND"))
                If Not IsDBNull(dt.Rows(0).Item("UM_NEXT_EXPIRE_DT")) Then
                    objUserDetail.NextExpiredDt = CStr(dt.Rows(0).Item("UM_NEXT_EXPIRE_DT"))
                End If
                If Not IsDBNull(dt.Rows(0).Item("UM_LAST_LOGIN_DT")) Then
                    objUserDetail.LastLogin = dt.Rows(0).Item("UM_LAST_LOGIN_DT")
                End If
                objUserDetail.Question = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_QUESTION")), "-1", dt.Rows(0).Item("UM_QUESTION")))
                objUserDetail.Answer = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_ANSWER")), "-1", dt.Rows(0).Item("UM_ANSWER")))
                objUserDetail.Status = CStr(dt.Rows(0).Item("UM_STATUS"))
                objUserDetail.PageCount = IIf(IsDBNull(dt.Rows(0).Item("UM_RECORD_COUNT")), 10, dt.Rows(0).Item("UM_RECORD_COUNT"))
                objUserDetail.CCList = Common.parseNull(dt.Rows(0).Item("UM_EMAIL_CC"))
                objUserDetail.StockTypeSpot = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_SPOT")), "N", dt.Rows(0).Item("UM_STK_TYPE_SPOT")))
                objUserDetail.StockTypeStock = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_STOCK")), "N", dt.Rows(0).Item("UM_STK_TYPE_STOCK")))
                objUserDetail.StockTypeMro = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_MRO")), "N", dt.Rows(0).Item("UM_STK_TYPE_MRO")))
                objUserDetail.SCEmailOnOff = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STAFF_CLAIM_EMAIL")), "N", dt.Rows(0).Item("UM_STAFF_CLAIM_EMAIL")))
                objUserDetail.PolicyAgreeDt = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_POLICY_AGREE_DATE")), "", dt.Rows(0).Item("UM_POLICY_AGREE_DATE")))
                dt.Clear()

                'strSQL = " SELECT * FROM USERS_USRGRP WHERE UU_USER_ID ='" & pUserID & "' AND UU_COY_ID='" & pCompID & "'"
                strSQL = " SELECT * FROM USERS_USRGRP left join USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID WHERE UU_USER_ID ='" & Common.Parse(pUserID) & "' AND UU_COY_ID='" & Common.Parse(pCompID) & "'"
                tDS = objDb.FillDs(strSQL)
                Dim strUserGroup As String = ""
                Dim strUserGroupName As String = ""
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    strUserGroup &= tDS.Tables(0).Rows(j).Item("UU_USRGRP_ID") & ","
                    strUserGroupName &= tDS.Tables(0).Rows(j).Item("UGM_USRGRP_NAME") & ","

                Next

                If Len(strUserGroup) > 0 Then
                    objUserDetail.UserGroup = Left(strUserGroup, Len(strUserGroup) - 1)
                End If

                If Len(strUserGroupName) > 0 Then
                    objUserDetail.UserGroupName = Left(strUserGroupName, Len(strUserGroupName) - 1)
                End If
                GetUserDetails = objUserDetail
            Else
                dt = Nothing
                objDcom = Nothing
                GetUserDetails = Nothing
            End If
        End Function

        'Name       : GetUpdateUserDetails
        'Author     : Umair
        'Descption  : Get all the User Particular and Update Login time 
        'Remark     : 
        'ReturnValue: UserDetail class
        'LastUpadte : 14 Mar 2018
        Public Function GetUpdateUserDetails(ByVal pUserID As String, ByVal pCompID As String) As User
            Dim strSQL As String
            Dim tDS As DataSet
            Dim objDcom As New EAD.DBCom
            strSQL = "UPDATE USER_MSTR SET UM_LAST_LOGIN_DT =" & Common.ConvertDate(Now) &
                         " WHERE UM_USER_ID ='" & Common.Parse(pUserID) & "' AND UM_COY_ID='" & Common.Parse(pCompID) & "';Select *, IFNULL(UM_PO_APP_LIMIT,0.00) As UM_PO_APP,  CAST(UM_PASSWORD As VARCHAR(100)) As PWD FROM USER_MSTR LEFT JOIN COMPANY_DEPT_MSTR On UM_DEPT_ID = CDM_DEPT_CODE WHERE UM_USER_ID='" & Common.Parse(pUserID) & "' AND UM_COY_ID='" & Common.Parse(pCompID) & "'  AND (UM_DELETED = 'N' OR UM_DELETED = 'S' ) "
            Dim dt As DataTable = objDcom.FillDt(strSQL)
            Dim objUserDetail As New User
            If dt.Rows.Count > 0 Then
                objUserDetail.UserID = CStr(dt.Rows(0).Item("UM_USER_ID"))
                objUserDetail.DeleteInd = CStr(dt.Rows(0).Item("UM_DELETED"))
                'objUserDetail.Password = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_PASSWORD")), "", dt.Rows(0).Item("UM_PASSWORD")))
                objUserDetail.Password = CStr(IIf(IsDBNull(dt.Rows(0).Item("PWD")), "", dt.Rows(0).Item("PWD")))
                objUserDetail.Name = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_USER_NAME")), "", dt.Rows(0).Item("UM_USER_NAME")))
                objUserDetail.CompanyID = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_COY_ID")), "", dt.Rows(0).Item("UM_COY_ID")))
                objUserDetail.DeptID = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_DEPT_ID")), "", dt.Rows(0).Item("UM_DEPT_ID")))
                objUserDetail.DeptName = CStr(IIf(IsDBNull(dt.Rows(0).Item("CDM_DEPT_NAME")), "", dt.Rows(0).Item("CDM_DEPT_NAME")))
                objUserDetail.Email = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_EMAIL")), "", dt.Rows(0).Item("UM_EMAIL")))
                'Michelle (20/9/2010) - To eliminate the deciamal points
                objUserDetail.ApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_APP_LIMIT")), "", dt.Rows(0).Item("UM_APP_LIMIT")))
                objUserDetail.InvoiceApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")), "", dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")))
                objUserDetail.ApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_APP_LIMIT")), "", Val(dt.Rows(0).Item("UM_APP_LIMIT"))))
                objUserDetail.POApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_PO_APP")), "", Val(dt.Rows(0).Item("UM_PO_APP"))))
                objUserDetail.InvoiceApproveLimit = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT")), "", Val(dt.Rows(0).Item("UM_INVOICE_APP_LIMIT"))))
                objUserDetail.MassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_MASS_APP")), "N", dt.Rows(0).Item("UM_MASS_APP")))
                objUserDetail.InvoiceMassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_INVOICE_MASS_APP")), "N", dt.Rows(0).Item("UM_INVOICE_MASS_APP")))
                objUserDetail.MrsMassApp = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_MRS_MASS_APP")), "N", dt.Rows(0).Item("UM_MRS_MASS_APP")))
                objUserDetail.Designation = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_DESIGNATION")), "", dt.Rows(0).Item("UM_DESIGNATION")))
                objUserDetail.PhoneNo = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_TEL_NO")), "", dt.Rows(0).Item("UM_TEL_NO")))
                objUserDetail.FaxNo = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_FAX_NO")), "", dt.Rows(0).Item("UM_FAX_NO")))
                objUserDetail.NewPwdInd = CStr(dt.Rows(0).Item("UM_NEW_PASSWORD_IND"))
                If Not IsDBNull(dt.Rows(0).Item("UM_NEXT_EXPIRE_DT")) Then
                    objUserDetail.NextExpiredDt = CStr(dt.Rows(0).Item("UM_NEXT_EXPIRE_DT"))
                End If
                If Not IsDBNull(dt.Rows(0).Item("UM_LAST_LOGIN_DT")) Then
                    objUserDetail.LastLogin = dt.Rows(0).Item("UM_LAST_LOGIN_DT")
                End If
                objUserDetail.Question = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_QUESTION")), "-1", dt.Rows(0).Item("UM_QUESTION")))
                objUserDetail.Answer = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_ANSWER")), "-1", dt.Rows(0).Item("UM_ANSWER")))
                objUserDetail.Status = CStr(dt.Rows(0).Item("UM_STATUS"))
                objUserDetail.PageCount = IIf(IsDBNull(dt.Rows(0).Item("UM_RECORD_COUNT")), 10, dt.Rows(0).Item("UM_RECORD_COUNT"))
                objUserDetail.CCList = Common.parseNull(dt.Rows(0).Item("UM_EMAIL_CC"))
                objUserDetail.StockTypeSpot = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_SPOT")), "N", dt.Rows(0).Item("UM_STK_TYPE_SPOT")))
                objUserDetail.StockTypeStock = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_STOCK")), "N", dt.Rows(0).Item("UM_STK_TYPE_STOCK")))
                objUserDetail.StockTypeMro = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STK_TYPE_MRO")), "N", dt.Rows(0).Item("UM_STK_TYPE_MRO")))
                objUserDetail.SCEmailOnOff = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_STAFF_CLAIM_EMAIL")), "N", dt.Rows(0).Item("UM_STAFF_CLAIM_EMAIL")))
                objUserDetail.PolicyAgreeDt = CStr(IIf(IsDBNull(dt.Rows(0).Item("UM_POLICY_AGREE_DATE")), "", dt.Rows(0).Item("UM_POLICY_AGREE_DATE")))
                dt.Clear()
                'strSQL = " SELECT * FROM USERS_USRGRP WHERE UU_USER_ID ='" & pUserID & "' AND UU_COY_ID='" & pCompID & "'"
                strSQL = " SELECT * FROM USERS_USRGRP left join USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID WHERE UU_USER_ID ='" & Common.Parse(pUserID) & "' AND UU_COY_ID='" & Common.Parse(pCompID) & "'"
                tDS = objDb.FillDs(strSQL)
                Dim strUserGroup As String = ""
                Dim strUserGroupName As String = ""
                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                    strUserGroup &= tDS.Tables(0).Rows(j).Item("UU_USRGRP_ID") & ","
                    strUserGroupName &= tDS.Tables(0).Rows(j).Item("UGM_USRGRP_NAME") & ","
                Next
                If Len(strUserGroup) > 0 Then
                    objUserDetail.UserGroup = Left(strUserGroup, Len(strUserGroup) - 1)
                End If
                If Len(strUserGroupName) > 0 Then
                    objUserDetail.UserGroupName = Left(strUserGroupName, Len(strUserGroupName) - 1)
                End If
                GetUpdateUserDetails = objUserDetail
            Else
                dt = Nothing
                objDcom = Nothing
                GetUpdateUserDetails = Nothing
            End If
        End Function

        'Public Function getAllAddr(ByVal pAddrType As String, ByVal role As String) As DataSet
        '    Dim strSql As String
        '    Dim objDB As New EAD.DBCom
        '    Dim ds As New DataSet
        '    If role = "B" Then
        '        strSql = "SELECT am_addr_index FROM address_mstr a WHERE am_addr_type='" & pAddrType & "' " &
        '        "AND am_coy_id='" & HttpContext.Current.Session("CompanyId") & "' "
        '    Else
        '        strSql = "SELECT am_addr_index FROM address_mstr a WHERE am_addr_type='" & pAddrType & "' " &
        '        "AND am_coy_id='" & HttpContext.Current.Session("CompanyId") & "' "
        '    End If

        '    ds = objDB.FillDs(strSql)
        '    Return ds
        'End Function
        'Public Function getSelectedAddr(ByVal pAddrType As String, ByVal user As String, ByVal role As String) As DataSet
        '    Dim strSql As String
        '    Dim objDB As New EAD.DBCom
        '    Dim ds As New DataSet
        '    If role = "B" Then
        '        strSql = "SELECT UA_USER_ID FROM ADDRESS_MSTR LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' " &
        '        "RIGHT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' " &
        '        "RIGHT JOIN USERS_ADDR ON AM_ADDR_CODE = UA_ADDR_CODE AND AM_COY_ID = UA_COY_ID " &
        '        "AND UA_USER_ID = '" & user & "' AND UA_ADDR_TYPE = AM_ADDR_TYPE WHERE AM_ADDR_TYPE='" & pAddrType & "' " &
        '        "AND am_coy_id='" & HttpContext.Current.Session("CompanyId") & "' "
        '    Else
        '        strSql = "SELECT UL_USER_ID FROM ADDRESS_MSTR LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' " &
        '        "RIGHT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' " &
        '        "RIGHT JOIN USERS_LOCATION ON AM_ADDR_CODE = UL_ADDR_CODE AND AM_COY_ID = UL_COY_ID " &
        '        "AND UL_USER_ID = '" & user & "' WHERE AM_ADDR_TYPE='" & pAddrType & "' " &
        '        "AND am_coy_id='" & HttpContext.Current.Session("CompanyId") & "' "
        '    End If

        '    ds = objDB.FillDs(strSql)
        '    Return ds
        'End Function
        'Michelle (6/1/2011)
        'Public Function getUsrWithAddr(ByVal pAddrType As String, Optional ByVal blnFTN As Boolean = True) As DataView

        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom
        '    'strSql = "SELECT  CONCAT(UM_USER_ID, ' : ' , UM_USER_NAME) AS USER, UM_USER_ID " & _
        '    '         "FROM USERS_USRGRP, USER_MSTR, USER_GROUP_MSTR " & _
        '    '         "WHERE UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " & _
        '    '         "AND UM_DELETED='N' AND UGM_DELETED='N' And UM_USER_ID = UU_USER_ID And UM_COY_ID = UU_COY_ID And "
        '    'Modified by Joon on 4 Oct 2011 for issue 959
        '    strSql = "SELECT DISTINCT CONCAT(UM_USER_ID, ' : ' , UM_USER_NAME) AS USER, UM_USER_ID " &
        '             "FROM USERS_USRGRP, USER_MSTR, USER_GROUP_MSTR " &
        '             "WHERE UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
        '             "AND UM_DELETED='N' AND UGM_DELETED='N' And UM_USER_ID = UU_USER_ID And UM_COY_ID = UU_COY_ID And "

        '    'Michelle (18/7/2011) - To add in Buyer fixed role, ie regardless whether it is for FTN or non-FTN
        '    'If blnFTN = True Then   'FTN
        '    '    If pAddrType = "D" Then 'Delivery Address 
        '    '        strSql &= "UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_FIXED_ROLE IN ('" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Store_Keeper.ToString, "_", " ") & "') "
        '    '    Else 'Billing Address
        '    '        strSql &= "UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_FIXED_ROLE IN ('" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "') "
        '    '    End If
        '    'Else    'Non FTN
        '    If pAddrType = "D" Then 'Delivery Address
        '        strSql &= "UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_FIXED_ROLE IN ('" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Store_Keeper.ToString, "_", " ") & "','" & Replace(FixedRole.Buyer.ToString, "_", " ") & "') "
        '    Else 'Billing Address
        '        strSql &= "UU_USRGRP_ID = UGM_USRGRP_ID AND UGM_FIXED_ROLE IN ('" & Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") & "','" & Replace(FixedRole.Purchasing_Manager.ToString, "_", " ") & "','" & Replace(FixedRole.Buyer.ToString, "_", " ") & "') "
        '    End If
        '    'End If

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Public Function getUsr(Optional ByVal blnFinance As Boolean = False) As DataView
        '    'Added by Joon on 21 June 2011
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    If blnFinance = True Then   'Finance Viewing Department
        '        strSql = "SELECT CONCAT( UM_USER_ID, ' : ' , UM_USER_NAME) AS USER, UM_USER_ID " _
        '                & "FROM USER_GROUP_MSTR, USERS_USRGRP, user_mstr " _
        '                & "WHERE UGM_USRGRP_ID = UU_USRGRP_ID And UU_USER_ID = UM_USER_ID " _
        '                & "AND UM_COY_ID = UU_COY_ID " _
        '                & "AND UGM_FIXED_ROLE IN ('" & Common.Parse(Replace(FixedRole.Finance_Manager.ToString, "_", " ")) & "','" & Common.Parse(Replace(FixedRole.Finance_Officer.ToString, "_", " ")) & "') " _
        '                & "AND UGM_DELETED='N' " _
        '                & "AND UU_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND UM_DELETED='N' " _
        '                & "ORDER BY UM_USER_ID ASC "


        '    Else
        '        strSql = "SELECT CONCAT( UM_USER_ID, ' : ' , UM_USER_NAME) AS USER, UM_USER_ID " _
        '                & "FROM USER_GROUP_MSTR, USERS_USRGRP, user_mstr " _
        '                & "WHERE UGM_USRGRP_ID = UU_USRGRP_ID And UU_USER_ID = UM_USER_ID " _
        '                & "AND UM_COY_ID = UU_COY_ID " _
        '                & "AND UGM_FIXED_ROLE ='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
        '                & "AND UGM_DELETED='N' " _
        '                & "AND UU_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND UM_DELETED='N' " _
        '                & "ORDER BY UM_USER_ID ASC "

        '    End If
        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        Public Function GetFixedRole(ByVal strGId As String) As String
            'Added by Joon on 22th June 2011
            Dim strSql As String

            strSql = "SELECT UGM_FIXED_ROLE FROM USER_GROUP_MSTR WHERE UGM_USRGRP_ID = '" & Common.Parse(strGId) & "'"
            If objDb.Exist(strSql) > 0 Then
                GetFixedRole = objDb.GetVal(strSql)
            Else
                GetFixedRole = ""
            End If
        End Function

        Public Function insertCommodityType(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            'Insert new record(s)
            For i = 0 To dtItem.Rows.Count - 1
                strsql = "SELECT '*' " _
                   & "FROM user_assign " _
                   & "WHERE UA_COY_ID='" & Common.Parse(dtItem.Rows(i).Item("UA_COY_ID")) & "' " _
                   & "AND UA_USER_ID='" & Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "' " _
                   & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
                   & "AND UA_ASSIGN_VALUE=" & dtItem.Rows(i).Item("UA_ASSIGN_VALUE")
                If objDb.Exist(strsql) = 0 Then
                    strsql = "INSERT INTO user_assign(" &
                           "UA_COY_ID,UA_USER_ID,UA_FIXED_ROLE,UA_TYPE,UA_ASSIGN_VALUE) " &
                           "VALUES('" &
                           Common.Parse(dtItem.Rows(i).Item("UA_COY_ID")) & "','" &
                           Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "', '" &
                           Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "','" &
                           Common.Parse(dtItem.Rows(i).Item("UA_TYPE")) & "'," &
                           dtItem.Rows(i).Item("UA_ASSIGN_VALUE") & ")"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    insertCommodityType = WheelMsgNum.Save
                Else
                    insertCommodityType = WheelMsgNum.NotSave
                End If
            Else
                insertCommodityType = WheelMsgNum.Save
            End If

        End Function

        Public Function RemoveCommodityType(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To dtItem.Rows.Count - 1
                strsql = "DELETE FROM user_assign " _
                        & "WHERE UA_COY_ID='" & Common.Parse(dtItem.Rows(i).Item("UA_COY_ID")) & "' " _
                        & "AND UA_USER_ID='" & Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "' " _
                        & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
                        & "AND UA_ASSIGN_VALUE=" & dtItem.Rows(i).Item("UA_ASSIGN_VALUE")
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    RemoveCommodityType = WheelMsgNum.Delete
                Else
                    RemoveCommodityType = WheelMsgNum.NotDelete
                End If
            End If

        End Function

        Public Function insertSection(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            'Insert new record(s)
            For i = 0 To dtItem.Rows.Count - 1
                strsql = "SELECT '*' " _
                   & "FROM COMPANY_SECTION_BUYER " _
                   & "WHERE CSB_SECTION_INDEX='" & Common.Parse(dtItem.Rows(i).Item("UA_SEC_INDEX")) & "' " _
                   & "AND CSB_SECTION_BUYER='" & Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "' "

                If objDb.Exist(strsql) = 0 Then
                    strsql = "INSERT INTO COMPANY_SECTION_BUYER(" &
                           "CSB_SECTION_INDEX,CSB_SECTION_BUYER) " &
                           "VALUES('" &
                           Common.Parse(dtItem.Rows(i).Item("UA_SEC_INDEX")) & "','" &
                           Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "')"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    insertSection = WheelMsgNum.Save
                Else
                    insertSection = WheelMsgNum.NotSave
                End If
            Else
                insertSection = WheelMsgNum.Save
            End If

        End Function

        Public Function RemoveSection(ByVal dtItem As DataTable) As Integer
            'Added by Joon on 22th June 2011
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            For i = 0 To dtItem.Rows.Count - 1
                strsql = "DELETE FROM COMPANY_SECTION_BUYER " _
                        & "WHERE CSB_SECTION_INDEX='" & Common.Parse(dtItem.Rows(i).Item("UA_SEC_INDEX")) & "' " _
                        & "AND CSB_SECTION_BUYER='" & Common.Parse(dtItem.Rows(i).Item("UA_USER_ID")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If strAryQuery(0) <> String.Empty Then
                If objDb.BatchExecute(strAryQuery) Then
                    RemoveSection = WheelMsgNum.Delete
                Else
                    RemoveSection = WheelMsgNum.NotDelete
                End If
            End If

        End Function

        'Public Function GetCommodity(ByVal strUser As String) As DataSet
        '    'Added by Joon on 21 June 2011
        '    Dim strSql As String
        '    Dim dt1 As New DataTable
        '    Dim intTmpPId As Integer = 0
        '    Dim i As Integer = 0
        '    Dim strTmpParent As String = ""
        '    Dim intCnt As Integer = 0
        '    Dim intPId As Integer = 0
        '    Dim blnHaveParent As Boolean
        '    Dim strTemp As String = ""
        '    Dim dt As New DataTable
        '    Dim dsTemp As New DataSet
        '    Dim strExist As String = ""

        '    strSql = "SELECT CT_ID,CT_NAME,CT_PARENT_ID FROM commodity_type WHERE CT_LAST_LVL=1"
        '    dt1 = objDb.FillDt(strSql)
        '    For i = 0 To dt1.Rows.Count - 1
        '        intCnt = 0
        '        blnHaveParent = True
        '        strExist = ""
        '        strTemp = dt1.Rows(i).Item("CT_NAME")
        '        Do While blnHaveParent = True
        '            If intCnt = 0 Then
        '                blnHaveParent = GetParent(dt1.Rows(i).Item("CT_PARENT_ID"), strTmpParent, intTmpPId)

        '            Else
        '                intPId = intTmpPId
        '                intTmpPId = 0
        '                strTmpParent = ""
        '                blnHaveParent = GetParent(intPId, strTmpParent, intTmpPId)
        '            End If
        '            strTemp = strTmpParent & " | " & strTemp
        '            intCnt = intCnt + 1
        '        Loop
        '        strExist = IsItemExist(dt1.Rows(i).Item("CT_ID"), strUser)
        '        dt = BindItem(strTemp, dt1.Rows(i).Item("CT_ID"), strTmpParent, strExist)
        '        If i = 0 Then
        '            dsTemp.Tables.Add(dt)
        '        Else
        '            dsTemp.Tables(0).Merge(dt)
        '        End If
        '    Next

        '    GetCommodity = dsTemp
        'End Function

        'Public Function GetCommodity(ByVal strUser As String) As DataSet
        '    'Added by Joon on 21 June 2011
        '    Dim strSql As String
        '    Dim dt1 As New DataTable
        '    Dim intTmpPId As Integer = 0
        '    Dim i As Integer = 0
        '    Dim dt As New DataTable
        '    Dim dsTemp As New DataSet
        '    Dim strExist As String = ""

        '    strSql = "SELECT CT_ROOT_PREFIX,CT_NAME,CT_PARENT_ID,CT_ID FROM commodity_type WHERE CT_PARENT_ID IS NULL"

        '    dt1 = objDb.FillDt(strSql)
        '    For i = 0 To dt1.Rows.Count - 1
        '        strExist = ""
        '        strExist = IsItemExist(dt1.Rows(i).Item("CT_ROOT_PREFIX"), strUser)
        '        dt = BindItem(dt1.Rows(i).Item("CT_NAME"), dt1.Rows(i).Item("CT_ID"), dt1.Rows(i).Item("CT_ROOT_PREFIX"), strExist)
        '        If i = 0 Then
        '            dsTemp.Tables.Add(dt)
        '        Else
        '            dsTemp.Tables(0).Merge(dt)
        '        End If
        '    Next

        '    GetCommodity = dsTemp
        'End Function

        'Public Function IsItemExist(ByVal strCTId As Integer, ByVal strUser As String) As Char
        '    'Added by Joon on 21 June 2011
        '    Dim strSql As String

        '    strSql = "SELECT '*' " _
        '        & "FROM user_assign " _
        '        & "WHERE UA_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
        '        & "AND UA_USER_ID='" & Common.Parse(strUser) & "' " _
        '        & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
        '        & "AND UA_ASSIGN_VALUE='" & strCTId & "'"

        '    If objDb.Exist(strSql) > 0 Then
        '        IsItemExist = "Y"
        '    Else
        '        IsItemExist = "N"
        '    End If
        'End Function

        Public Function BindItem(ByVal strCTName As String, ByVal intCTId As Integer, ByVal strPrefix As String, ByVal strExist As String) As DataTable
            'Added by Joon on 21 June 2011
            Dim dtItem As New DataTable
            Dim dtr As DataRow

            dtItem.Columns.Add("CT_NAME", Type.GetType("System.String"))
            dtItem.Columns.Add("CT_ID", Type.GetType("System.String"))
            dtItem.Columns.Add("CT_ROOT_PREFIX", Type.GetType("System.String"))
            dtItem.Columns.Add("AM_SELECTED", Type.GetType("System.String"))

            dtr = dtItem.NewRow()
            dtr("CT_NAME") = strCTName
            dtr("CT_ID") = intCTId
            dtr("CT_ROOT_PREFIX") = strPrefix
            dtr("AM_SELECTED") = strExist
            dtItem.Rows.Add(dtr)

            If dtItem.Rows.Count > 0 Then
                BindItem = dtItem
            End If
        End Function

        'Public Function GetParent(ByVal intParentId As Integer, ByRef strParent As String, ByRef intPId As Integer) As Boolean
        '    'Added by Joon on 21 June 2011
        '    Dim strSql As String
        '    Dim ds As New DataSet

        '    strSql = "SELECT CT_ID,CT_NAME,CT_PARENT_ID FROM commodity_type WHERE CT_ID = " & intParentId & " AND CT_LAST_LVL <> 1"
        '    ds = objDB.FillDs(strSql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        If Not IsDBNull(ds.Tables(0).Rows(0).Item("CT_PARENT_ID")) Then
        '            intPId = ds.Tables(0).Rows(0).Item("CT_PARENT_ID")
        '            GetParent = True
        '        Else
        '            intPId = 0
        '            GetParent = False
        '        End If
        '        strParent = ds.Tables(0).Rows(0).Item("CT_NAME")
        '    End If
        '    ds = Nothing

        'End Function

        Public Function GetAdmin(ByVal pCoyId As String) As String
            Dim strSQL, strRec As String
            Dim ds As New DataSet

            If pCoyId <> "" Then
                'Dim drdRAppCommoner As OleDb.OleDbDataRAppCommoner

                strSQL = "SELECT DISTINCT(UM_USER_ID),UM_USER_NAME,UM_EMAIL " &
                   " FROM USER_MSTR, USERS_USRGRP, USER_GROUP_MSTR " &
                   " WHERE(UM_COY_ID = UU_COY_ID And UM_USER_ID = UU_USER_ID)" &
                   " AND (UU_USRGRP_ID=UGM_USRGRP_ID AND UGM_DELETED<>'Y' " &
                   " AND UGM_FIXED_ROLE IN ('Vendor Administrator','Buyer Administrator')) " &
                   " AND UM_COY_ID='" & pCoyId & "'"

                ds = objDb.FillDs(strSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        strRec &= ds.Tables(0).Rows(i).Item("UM_EMAIL") & ","
                    Next
                End If

                If strRec <> "" Then
                    Return Mid(strRec, 1, Len(strRec) - 1)
                Else
                    Return ""
                End If
            End If

            ds = Nothing
        End Function

        ' check if user doesn't select any record for current page but alrAppCommony saved records from other page
        'Public Function checkAddrSelected(ByVal strUserId As String, ByVal strType As String, ByVal strAddr As String, ByVal strLevel As String) As Boolean
        '    Dim strsql As String
        '    Select Case strType
        '        Case "SK"
        '            ' strLevel : 1 - SK, 2 - 2nd Level
        '            strsql = "SELECT '*' FROM USERS_LOCATION "
        '            strsql &= "WHERE UL_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '            strsql &= "AND UL_USER_ID = '" & strUserId & "' "
        '            strsql &= "AND UL_LEVEL = " & strLevel & " "
        '            strsql &= "AND UL_ADDR_CODE NOT IN (" & strAddr & ") "
        '        Case "B"
        '            ' strLevel : B - Billing, D - Delivery
        '            strsql = "SELECT '*' FROM USERS_ADDR "
        '            strsql &= "WHERE UA_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '            strsql &= "AND UA_USER_ID = '" & strUserId & "' "
        '            strsql &= "AND UA_ADDR_TYPE = '" & strLevel & "' "
        '            strsql &= "AND (UA_ADDR_CODE NOT IN (" & strAddr & ") AND UA_VIEW_OPTION = 0) "
        '    End Select

        '    If objDb.Exist(strsql) > 0 Then
        '        checkAddrSelected = True
        '    Else
        '        checkAddrSelected = False
        '    End If
        'End Function

        'Public Function GetUserDetail2(ByVal strID As String, ByVal objUser As User)
        '    Dim lsSql As String
        '    Dim ds As DataSet
        '    lsSql = "SELECT * FROM USER_MSTR WHERE UM_USER_ID='" & strID & "'"
        '    Try
        '        '//Cannot use objDB.getview because it may return nothing 
        '        '//and we can't assign nothing to object
        '        ds = objDb.FillDs(lsSql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            'GetUserDetail2 = PopulateUser(ds.Tables(0).Rows(0))
        '            objUser.Name = ds.Tables(0).Rows(0)("UM_USER_NAME")
        '            objUser.PhoneNo = ds.Tables(0).Rows(0)("UM_TEL_NO")
        '        Else
        '            '   Return Nothing
        '        End If
        '        'Dim dv As DataView
        '        ' dv = objDB.GetView(lsSql)
        '        'GetUserDetail = PopulateUser(dv.Table.Rows(0))
        '        '//to be continued
        '    Catch ErrExp As Exception
        '        Common.TrwExp(ErrExp)
        '    Finally
        '        objDb = Nothing
        '    End Try

        'End Function

        'Name       : SearchUser
        'Author     : kk
        'Descption  : Search all undelete user
        'LastUpadte : 22/11/2004
        'Public Function SearchUser(ByVal pUserID As String, ByVal pUserName As String, ByVal pHubLevel As Boolean, Optional ByVal pShowAdmin As Boolean = False) As DataSet
        '    Dim strsql_user, strsql_user2 As String
        '    Dim strCompany As String
        '    Dim dsUser As DataSet
        '    Dim strTemp As String

        '    If pHubLevel Then
        '        strCompany = Common.Parse(ctx.Session("CompanyIdToken"))
        '    Else
        '        strCompany = Common.Parse(ctx.Session("CompanyId"))
        '    End If
        '    'Zulham 05112018
        '    strsql_user = "SELECT DISTINCT(UM_USER_ID),UM_USER_NAME,CDM_DEPT_NAME,UM_STATUS,UM_COY_ID,UM_DELETED,1 AS App" & IIf(pHubLevel, " ", ", UU_APP_PKG ") &
        '    "FROM USER_MSTR LEFT JOIN COMPANY_DEPT_MSTR ON  UM_DEPT_ID=CDM_DEPT_CODE AND UM_COY_ID=CDM_COY_ID, " &
        '    "COMPANY_MSTR,USERS_USRGRP LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID=UU_USRGRP_ID AND UGM_APP_PKG = UU_APP_PKG " &
        '    "WHERE UM_COY_ID=CM_COY_ID AND (UM_USER_ID=UU_USER_ID AND UM_COY_ID=UU_COY_ID) AND UM_DELETED <> 'Y' " &
        '    "AND CDM_DELETED='N' AND  " 'Jules 2018.11.02 - Added CDM_DELETED.

        '    If pUserID <> "" Then
        '        strTemp = Common.BuildWildCard(pUserID)
        '        strsql_user = strsql_user & " UPPER(UM_USER_ID)" & Common.ParseSQL(strTemp) & "AND "
        '    End If

        '    If pUserName <> "" Then
        '        strTemp = Common.BuildWildCard(pUserName)
        '        strsql_user = strsql_user & " UPPER(UM_USER_NAME)" & Common.ParseSQL(strTemp) & "AND "
        '    End If

        '    'Michelle (16/5/2012) - Issue 1614,1615
        '    'If pHubLevel Then
        '    If Not pShowAdmin Then
        '        If pHubLevel Then
        '            ' Only Admin account will be displayed
        '            'strsql_user &= " (UGM_FIXED_ROLE IN ('Vendor Administrator','Buyer Administrator') OR UU_USRGRP_ID='HubAdmin') AND UM_DELETED<>'Y' AND UM_COY_ID='" & Common.Parse(ctx.Session("CompanyIdToken")) & "' ORDER BY UM_USER_ID "
        '            strsql_user &= " (UGM_FIXED_ROLE IN ('" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "') OR UU_USRGRP_ID='HubAdmin') AND UM_DELETED<>'Y' AND UM_COY_ID='" & strCompany & "' " 'ORDER BY UM_USER_ID "
        '        Else
        '            'strsql_user &= " (UGM_FIXED_ROLE NOT IN ('" & FixedRole.Admin.ToString & "')) AND UM_DELETED<>'Y' AND UM_COY_ID='" & ctx.Session("CompanyId") & "' ORDER BY UM_USER_ID "
        '            strsql_user &= " (UGM_FIXED_ROLE NOT IN ('" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "')) AND UM_DELETED<>'Y' AND UM_COY_ID='" & strCompany & "' "
        '        End If
        '    Else
        '        strsql_user &= " UM_DELETED<>'Y' AND UM_COY_ID='" & strCompany & "' "
        '    End If
        '    'Zulham 05112018
        '    ' to bind user not in current system
        '    strsql_user2 = "SELECT UM_USER_ID,UM_USER_NAME,CDM_DEPT_NAME,UM_STATUS,UM_COY_ID,UM_DELETED, 0 AS App" & IIf(pHubLevel, " ", ", '' AS UU_APP_PKG ")
        '    strsql_user2 &= "FROM USER_MSTR LEFT JOIN COMPANY_DEPT_MSTR ON  UM_DEPT_ID=CDM_DEPT_CODE AND UM_COY_ID=CDM_COY_ID "
        '    strsql_user2 &= "WHERE UM_COY_ID = '" & strCompany & "' AND CDM_DELETED='N' AND UM_DELETED<>'Y' "
        '    strsql_user2 &= "AND UM_USER_ID NOT IN (SELECT DISTINCT UU_USER_ID FROM USERS_USRGRP WHERE UU_COY_ID = '" & strCompany & "') "

        '    strsql_user &= " UNION " & strsql_user2

        '    'Zulham 04112018
        '    If pUserID <> "" Then
        '        strTemp = Common.BuildWildCard(pUserID)
        '        strsql_user = strsql_user & " AND UPPER(UM_USER_ID)" & Common.ParseSQL(strTemp) & " "
        '    End If

        '    If pUserName <> "" Then
        '        strTemp = Common.BuildWildCard(pUserName)
        '        strsql_user = strsql_user & "AND UPPER(UM_USER_NAME)" & Common.ParseSQL(strTemp) & " "
        '    End If

        '    'Zulham 04112018
        '    Dim strsql_user3 As String = ""

        '    'Jules 2018.12.18 - SQL error: "The used SELECT statements have a different number of columns"
        '    'strsql_user3 = "UNION ALL Select DISTINCT UM_USER_ID, UM_USER_NAME, '' AS CDM_DEPT_NAME, UM_STATUS, UM_COY_ID,UM_DELETED, 1 AS App, UU_APP_PKG
        '    strsql_user3 = "UNION ALL Select DISTINCT UM_USER_ID, UM_USER_NAME, '' AS CDM_DEPT_NAME, UM_STATUS, UM_COY_ID,UM_DELETED, 1 AS App" & IIf(pHubLevel, " ", ", '' AS UU_APP_PKG ")
        '    'End modification.

        '    strsql_user3 &= "FROM USER_MSTR, COMPANY_MSTR, USERS_USRGRP  
        '                    LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID=UU_USRGRP_ID And UGM_APP_PKG = UU_APP_PKG 
        '                    WHERE UM_COY_ID = CM_COY_ID And (UM_USER_ID = UU_USER_ID And UM_COY_ID = UU_COY_ID) 
        '                    AND UM_DELETED<>'Y' AND UM_COY_ID='" & strCompany & "'  
        '                    AND (UM_DEPT_ID IS NULL OR TRIM(UM_DEPT_ID) = '') "

        '    If pUserID <> "" Then
        '        strTemp = Common.BuildWildCard(pUserID)
        '        strsql_user3 = strsql_user3 & " AND UPPER(UM_USER_ID)" & Common.ParseSQL(strTemp) & " "
        '    End If

        '    If pUserName <> "" Then
        '        strTemp = Common.BuildWildCard(pUserName)
        '        strsql_user3 = strsql_user3 & " AND UPPER(UM_USER_NAME)" & Common.ParseSQL(strTemp) & " "
        '    End If

        '    strsql_user &= strsql_user3

        '    strsql_user &= " AND UM_DELETED<>'Y' AND UM_COY_ID='" & strCompany & "' ORDER BY UM_USER_ID "

        '    dsUser = objDb.FillDs(strsql_user)
        '    SearchUser = dsUser
        'End Function

        'Public Function SearchAllUserSection(ByVal pUserID As String, ByVal pUserName As String, ByVal pSecCode As String, ByVal chkAssigned As String) As DataSet
        '    Dim strSql, strTemp As String
        '    Dim dsUserSec As DataSet

        '    strSql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, CDM_DEPT_NAME, UM_STATUS, UM_DELETED, " &
        '            "CASE WHEN CSB_SECTION_BUYER <> '' THEN 'Yes' ELSE 'No' END AS CSB_SECTION " &
        '            "FROM USER_MSTR UM " &
        '            "LEFT JOIN (SELECT CSB_SECTION_BUYER FROM COMPANY_SECTION_BUYER " &
        '            "WHERE CSB_SECTION_INDEX = '" & Common.Parse(pSecCode) & "') AS tb " &
        '            "ON tb.CSB_SECTION_BUYER = UM.UM_USER_ID " &
        '            "LEFT JOIN COMPANY_DEPT_MSTR ON UM_DEPT_ID = CDM_DEPT_CODE AND UM_COY_ID = CDM_COY_ID, " &
        '            "COMPANY_MSTR, USERS_USRGRP " &
        '            "LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID=UU_USRGRP_ID AND UGM_APP_PKG = UU_APP_PKG " &
        '            "WHERE UM_COY_ID = CM_COY_ID And (UM_USER_ID = UU_USER_ID And UM_COY_ID = UU_COY_ID) " &
        '            "AND UGM_FIXED_ROLE <> 'Super Admin' AND UM_COY_ID = '" & Common.Parse(ctx.Session("CompanyId")) & "' "

        '    If chkAssigned = "Y" Then
        '        strSql = strSql & " AND UM_USER_ID IN (SELECT CSB_SECTION_BUYER FROM COMPANY_SECTION_BUYER WHERE CSB_SECTION_INDEX = '" & Common.Parse(pSecCode) & "') "
        '    Else
        '        strSql = strSql & " AND NOT UM_USER_ID IN (SELECT CSB_SECTION_BUYER FROM COMPANY_SECTION_BUYER WHERE CSB_SECTION_INDEX = '" & Common.Parse(pSecCode) & "') "
        '    End If

        '    If pUserID <> "" Then
        '        strTemp = Common.BuildWildCard(pUserID)
        '        strSql = strSql & " AND UM_USER_ID " & Common.ParseSQL(strTemp)
        '    End If

        '    If pUserName <> "" Then
        '        strTemp = Common.BuildWildCard(pUserName)
        '        strSql = strSql & " AND UM_USER_NAME " & Common.ParseSQL(strTemp)
        '    End If

        '    dsUserSec = objDb.FillDs(strSql)
        '    SearchAllUserSection = dsUserSec
        'End Function

        'Public Function ValidUser(ByVal pCoyID As String, ByVal pUserId As String, ByVal pPwd As String) As Boolean
        '    Dim lsSql As String
        '    Dim strPwd As String

        '    'strPwd = EncryptString(pPwd, "[+-*/]")

        '    lsSql = "SELECT * FROM USERS_MSTR WHERE UM_COMPANY_ID='" & pCoyID & _
        '    "' AND UM_USER_ID='" & pUserId & "'"

        '    Dim drdData As OleDb.OleDbDataRAppCommoner = objDb.GetRAppCommoner(lsSql)
        '    If drdData.RAppCommon Then
        '        'strPwd = EncryptString(CStr(drdData("UM_PAWD")), "[+-*/]")
        '        strPwd = EncryptString(CStr(drdData("UM_PAWD")), "[+-*/]")
        '        If strPwd = pPwd Then
        '            'valid user
        '            '//IF pwd=system gen password,redirect to change pwd page
        '            '//
        '        Else
        '            'invalid user
        '            '//update no of attempt
        '            '//
        '        End If
        '    End If
        '    drdData = Nothing
        '    objDb = Nothing
        'End Function

        'Public Function Login(ByVal pUser_ID As String, ByVal pPassword As String, ByVal pCompID As String) As String

        '    Dim strSQL, strSQL_Comp As String
        '    Dim strPassword As String
        '    Dim ObjDcom As New EAD.DBCom
        '    Dim objLP As New LoginPolicy

        '    If ObjDcom.ConnState = False Then
        '        Return "@Connection"
        '    End If

        '    strSQL_Comp = "SELECT '*' From Company_MSTR where UPPER(CM_COY_ID)='" & Common.Parse(UCase(pCompID)) & "' AND CM_STATUS ='I' AND CM_DELETED <>'Y'"
        '    If ObjDcom.Exist(strSQL_Comp) > 0 Then
        '        strUserMassage = "Your company is inactive. Please contact your administrator."
        '        Return Nothing
        '    End If

        '    strSQL = " SELECT UM_AUTO_NO,UM_USER_ID,UM_PASSWORD,UM_USER_SUSPEND_IND,UM_STATUS,UM_DELETED, CAST(UM_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_MSTR,COMPANY_MSTR WHERE (CM_COY_ID=UM_COY_ID AND CM_STATUS='A' AND UM_DELETED<>'Y') AND UPPER(UM_USER_ID) ='" & Common.Parse(UCase(pUser_ID)) & "'" &
        '             " AND UPPER(UM_COY_ID)='" & Common.Parse(UCase(pCompID)) & "' AND UM_DELETED <>'Y'"

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        'Account is suspended
        '        If tDS.Tables(0).Rows(j).Item("UM_DELETED") = "S" Then
        '            'strUserMassage = "Your account is locked. Please contact your administrator."
        '            strUserMassage = "Your account is locked. Please click " + "'Forgot Your Password?' to reset password."
        '            Return Nothing
        '        End If

        '        'Account is inactive
        '        If tDS.Tables(0).Rows(j).Item("UM_STATUS") = "I" Then
        '            strUserMassage = "Your account is inactive. Please contact your administrator."
        '            Return Nothing
        '        End If

        '        'strPassword = EncryptString(CStr(drdRAppCommoner("UM_PASSWORD")), "[+-*/]")
        '        strPassword = tDS.Tables(0).Rows(j).Item("PWD")

        '        If strPassword = Common.EncryptString(pPassword) Then
        '            'If strPassword = pPassword Then
        '            If tDS.Tables(0).Rows(j).Item("UM_USER_SUSPEND_IND") <> 0 Then
        '                SuspendAc(pUser_ID, pCompID, 0)
        '            End If

        '            '//add by Moo, to override Session.TimeOut
        '            '//cannot put at Global.asax as Session.OnStart only called when lauching new IE
        '            Dim strSessionTimeOut As String
        '            Dim objLoginPolicy As New LoginPolicy
        '            strSessionTimeOut = Common.parseNull(objLoginPolicy.getParamValue("SESSION_TIMEOUT"), HttpContext.Current.Session.Timeout)
        '            objLoginPolicy = Nothing
        '            HttpContext.Current.Session.Timeout = CInt(strSessionTimeOut)
        '            Return CStr(tDS.Tables(0).Rows(j).Item("UM_USER_ID"))
        '        Else
        '            If objLP.getParamInd("LOGIN_FREQUENCY") = "YES" Then
        '                SuspendAc(pUser_ID, pCompID, tDS.Tables(0).Rows(j).Item("UM_USER_SUSPEND_IND") + 1) 'increase the try counter
        '            End If
        '            strUserMassage = "Invalid login, please double check the Company Id, User Id and password."
        '            Return Nothing
        '        End If
        '    Next

        '    strUserMassage = "Invalid login, please double check the Company Id, User Id and password."

        '    Return Nothing

        'End Function
        'Public Function SingleLogin(ByVal pUser_ID As String, ByVal pPassword As String, ByVal pCompID As String) As String

        '    Dim strSQL, strSQL_Comp As String
        '    Dim strPassword As String
        '    Dim ObjDcom As New EAD.DBCom
        '    Dim objLP As New LoginPolicy

        '    If ObjDcom.ConnState = False Then
        '        Return "@Connection"
        '    End If

        '    strSQL_Comp = "SELECT '*' From Company_MSTR where UPPER(CM_COY_ID)='" & Common.Parse(UCase(pCompID)) & "' AND CM_STATUS ='I' AND CM_DELETED <>'Y'"
        '    If ObjDcom.Exist(strSQL_Comp) > 0 Then
        '        strUserMassage = "Your company is inactive. Please contact your administrator."
        '        Return Nothing
        '    End If

        '    strSQL = " SELECT UM_AUTO_NO,UM_USER_ID,UM_PASSWORD,UM_USER_SUSPEND_IND,UM_STATUS,UM_DELETED, CAST(UM_PASSWORD AS VARCHAR(100)) AS PWD FROM USER_MSTR,COMPANY_MSTR WHERE (CM_COY_ID=UM_COY_ID AND CM_STATUS='A' AND UM_DELETED<>'Y') AND UPPER(UM_USER_ID) ='" & Common.Parse(UCase(pUser_ID)) & "'" &
        '             " AND UPPER(UM_COY_ID)='" & Common.Parse(UCase(pCompID)) & "' AND UM_DELETED <>'Y'"

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        'Account is suspended
        '        If tDS.Tables(0).Rows(j).Item("UM_DELETED") = "S" Then
        '            'strUserMassage = "Your account is locked. Please contact your administrator."
        '            strUserMassage = "Your account is locked. Please click " + "'Forgot Your Password?' to reset password."
        '            Return Nothing
        '        End If

        '        'Account is inactive
        '        If tDS.Tables(0).Rows(j).Item("UM_STATUS") = "I" Then
        '            strUserMassage = "Your account is inactive. Please contact your administrator."
        '            Return Nothing
        '        End If

        '        'strPassword = EncryptString(CStr(drdRAppCommoner("UM_PASSWORD")), "[+-*/]")
        '        strPassword = tDS.Tables(0).Rows(j).Item("PWD")
        '        If strPassword = pPassword Then
        '            'If strPassword = pPassword Then
        '            If tDS.Tables(0).Rows(j).Item("UM_USER_SUSPEND_IND") <> 0 Then
        '                SuspendAc(pUser_ID, pCompID, 0)
        '            End If

        '            '//add by Moo, to override Session.TimeOut
        '            '//cannot put at Global.asax as Session.OnStart only called when lauching new IE
        '            Dim strSessionTimeOut As String
        '            Dim objLoginPolicy As New LoginPolicy
        '            strSessionTimeOut = Common.parseNull(objLoginPolicy.getParamValue("SESSION_TIMEOUT"), HttpContext.Current.Session.Timeout)
        '            objLoginPolicy = Nothing
        '            HttpContext.Current.Session.Timeout = CInt(strSessionTimeOut)
        '            Return CStr(tDS.Tables(0).Rows(j).Item("UM_USER_ID"))
        '        Else
        '            If objLP.getParamInd("LOGIN_FREQUENCY") = "YES" Then
        '                SuspendAc(pUser_ID, pCompID, tDS.Tables(0).Rows(j).Item("UM_USER_SUSPEND_IND") + 1) 'increase the try counter
        '            End If
        '            strUserMassage = "Invalid login, please double check the Company Id, User Id and password."
        '            Return Nothing
        '        End If
        '    Next

        '    strUserMassage = "Invalid login, please double check the Company Id, User Id and password."

        '    Return Nothing

        'End Function

        'Name       : DeleteCompany
        'Author     : kk
        'Descption  : delete company
        'LastUpadte : 26/11/2004
        'Public Function ActivateUserAccount(ByVal pUserId As String, ByVal pBoo As Boolean, ByVal pHubLevel As Boolean) As Boolean
        '    Dim strUpdate, strCompId As String
        '    Dim dvComp As DataView
        '    Dim objUser As New Users
        '    Dim cInd As Char

        '    cInd = IIf(pBoo, "A", "I")

        '    Try
        '        If pHubLevel Then
        '            strCompId = ctx.Session("CompanyIdToken")
        '        Else
        '            strCompId = ctx.Session("CompanyId")
        '        End If

        '        If pBoo Then
        '            'This os to cater if the user update status, if chg from I -> A will required to chk
        '            'user limit
        '            Dim strSQL, strStatus As String
        '            strSQL = "SELECT UM_STATUS FROM USER_MSTR WHERE UM_USER_ID='" & Common.Parse(pUserId) &
        '                       "' AND UM_COY_ID='" & Common.Parse(strCompId) & "' AND UM_DELETED<>'Y'"
        '            strStatus = objDb.GetVal(strSQL)

        '            If pHubLevel Then
        '                If objUser.IsExceedAdminLimit(strCompId) And strStatus = "I" Then
        '                    strUserMassage = objUser.Message
        '                    Return False
        '                End If
        '            Else
        '                If objUser.IsExceedUserLimit(strCompId) And strStatus = "I" Then
        '                    strUserMassage = "The number of active users exceeds the User License."
        '                    Return False
        '                End If
        '                If IsExceedReportUser(strCompId, pUserId) And strStatus = "I" Then Return False
        '            End If
        '        End If

        '        strUpdate = "UPDATE USER_MSTR SET UM_STATUS='" & cInd & "'," &
        '        "UM_MOD_BY='" & ctx.Session("UserId") & "', " &
        '        "UM_MOD_DT=" & Common.ConvertDate(Now) &
        '        " WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & strCompId & "'"

        '        If objDb.Execute(strUpdate) Then
        '            strUserMassage = "User Account Activated."
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        'Public Function ActivateUserAccount(ByVal pCompanyId As String, ByVal pUserId As String, ByVal pBoo As Boolean) As Boolean
        '    Dim strUpdate As String
        '    Dim dvComp As DataView
        '    Dim objUser As New Users
        '    Dim cInd As Char

        '    cInd = IIf(pBoo, "A", "I")

        '    Try
        '        If pBoo Then
        '            'This os to cater if the user update status, if chg from I -> A will required to chk
        '            'user limit
        '            Dim strSQL, strStatus As String
        '            strSQL = "SELECT UM_STATUS FROM USER_MSTR WHERE UM_USER_ID='" & Common.Parse(pUserId) &
        '                       "' AND UM_COY_ID='" & Common.Parse(pCompanyId) & "' AND UM_DELETED<>'Y'"
        '            strStatus = objDb.GetVal(strSQL)

        '            If objUser.IsExceedUserLimit(pCompanyId) And strStatus = "I" Then
        '                strUserMassage = "The number of active users exceeds the User License."
        '                Return False
        '            End If
        '            If objUser.IsExceedReportUser(pCompanyId, pUserId) And strStatus = "I" Then Return False
        '        End If

        '        strUpdate = "UPDATE USER_MSTR SET UM_STATUS='" & cInd & "'," &
        '        "UM_MOD_BY='" & ctx.Session("UserId") & "', " &
        '        "UM_MOD_DT=" & Common.ConvertDate(Now) &
        '        " WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & pCompanyId & "'"

        '        If objDb.Execute(strUpdate) Then
        '            strUserMassage = "User Account Activated."
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        Public Function SuspendAc(ByVal pUser_ID As String, ByVal pCompId As String, ByVal pSupNo As Integer) As Boolean
            SuspendAc = True
            Dim strSQL As String
            Dim ObjDcom As New EAD.DBCom
            Dim ObjLP As New LoginPolicy
            Dim strSupNo As Integer = ObjLP.getParamValue("LOGIN_FREQUENCY")


            If pSupNo = 0 Then
                strSQL = " UPDATE USER_MSTR SET UM_USER_SUSPEND_IND =" & pSupNo & ",UM_DELETED ='N'" &
                         " WHERE UM_USER_ID ='" & Common.Parse(pUser_ID) & "' AND UM_COY_ID='" & Common.Parse(pCompId) & "'"
            ElseIf pSupNo = strSupNo Then
                strSQL = " UPDATE USER_MSTR SET UM_USER_SUSPEND_IND =" & pSupNo & ",UM_DELETED ='S'" &
                         " WHERE UM_USER_ID ='" & Common.Parse(pUser_ID) & "' AND UM_COY_ID='" & Common.Parse(pCompId) & "'"
            Else
                strSQL = " UPDATE USER_MSTR SET UM_USER_SUSPEND_IND =" & pSupNo &
                         " WHERE UM_USER_ID ='" & Common.Parse(pUser_ID) & "' AND UM_COY_ID='" & Common.Parse(pCompId) & "'"
            End If

            If ObjDcom.Execute(strSQL) <> True Then
                ObjDcom = Nothing
                SuspendAc = False
            End If


        End Function
        'Name       : 
        'Author     : Kk
        'Descption  : Update LogIn Date
        'Remark     : Input No should be new suspend no.
        'ReturnValue: User Id
        'LastUpadte : 09 Nov 2004
        'Version    : 1.00
        Public Function UpdateLoginDt(ByVal User As User) As Boolean

            Dim strSQL As String
            Dim ObjDcom As New EAD.DBCom


            strSQL = " UPDATE USER_MSTR SET UM_LAST_LOGIN_DT =" & Common.ConvertDate(Now) &
                         " WHERE UM_USER_ID ='" & User.UserID & "' AND UM_COY_ID='" & User.CompanyID & "'"

            Return ObjDcom.Execute(strSQL)

        End Function

        Public Function UpdatePolicyAgreeDt(ByVal UserID As String, ByVal pCompID As String) As Boolean

            Dim strSQL As String
            Dim ObjDcom As New EAD.DBCom


            strSQL = " UPDATE USER_MSTR SET UM_POLICY_AGREE_DATE =" & Common.ConvertDate(Now) &
                     " WHERE UM_USER_ID ='" & UserID & "' AND UM_COY_ID='" & pCompID & "'"

            Return ObjDcom.Execute(strSQL)

        End Function

        'kk
        '22/11/2004
        Public Function SearchJobRole(ByVal pJobID As String, ByVal pJobName As String) As DataSet
            Dim strsql_sdept As String
            Dim dssdept As DataSet
            strsql_sdept = "select * from JOBTITLE_HAppCommonER  " ' JH_JOBTITLE_ID='" & pJobName & "'" 'AND CDM_DELETED='N'"

            If pJobID <> "" Then
                strsql_sdept = strsql_sdept & " WHERE  UPPER(JH_JOBTITLE_ID)" & Common.ParseSQL(pJobID)
            End If

            If pJobName <> "" Then
                strsql_sdept = strsql_sdept & " WHERE  UPPER(JH_JOBTITLE_NAME)" & Common.ParseSQL(pJobName)
            End If

            dssdept = objDb.FillDs(strsql_sdept)
            SearchJobRole = dssdept
        End Function


        'Public Function GetUserGroup(ByVal pUserId As String, ByVal pCoyType As String, ByVal pHubLevel As Boolean, Optional ByVal pReverse As Boolean = False, Optional ByVal pexcFTN As Boolean = False, Optional ByVal pShowSuperAdmin As Boolean = False) As DataView
        '    Dim strsql_usergrp As String
        '    Dim dsusergrp As DataView
        '    Dim strCompId, strType As String

        '    If pHubLevel Then
        '        strCompId = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompId = ctx.Session("CompanyId")
        '    End If

        '    'Added condition for other company's types
        '    If pCoyType = "BOTH" Then
        '        strType = "UGM_TYPE IN ('BUYER','VENDOR')"
        '    ElseIf pCoyType = "BUYER" Then
        '        strType = "UGM_TYPE='BUYER'"
        '    ElseIf pCoyType = "VENDOR" Then
        '        strType = "UGM_TYPE='VENDOR'"
        '    ElseIf pCoyType = "HUB" Then
        '        strType = "UGM_TYPE='HUB'"
        '    Else
        '        strType = "UGM_TYPE='VENDOR'"
        '    End If


        '    If pReverse Then
        '        strsql_usergrp = " SELECT * FROM USER_GROUP_MSTR WHERE UGM_USRGRP_ID NOT IN " &
        '                         " (SELECT UU_USRGRP_ID FROM USERS_USRGRP " &
        '                         " WHERE(UU_COY_ID='" & Common.Parse(strCompId) & "' AND UU_USER_ID='" & Common.Parse(pUserId) & "')) AND  UGM_DELETED<>'Y' AND " & strType
        '    Else
        '        strsql_usergrp = " SELECT * FROM USER_GROUP_MSTR,USERS_USRGRP " &
        '                         "WHERE UGM_USRGRP_ID=UU_USRGRP_ID AND (UU_COY_ID='" & Common.Parse(strCompId) & "' AND UU_USER_ID='" & Common.Parse(pUserId) & "') AND UGM_DELETED<>'Y' AND " & strType
        '    End If

        '    If pexcFTN Then strsql_usergrp &= " AND SUBSTRING(UGM_USRGRP_ID,1,3) <> 'FTN'"

        '    'Michelle (13/8/2012) - Issue 1684
        '    'If Not pHubLevel Then
        '    If Not pHubLevel And Not pShowSuperAdmin Then
        '        strsql_usergrp &= " AND UGM_FIXED_ROLE <> '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "'"
        '    End If

        '    dsusergrp = objDb.GetView(strsql_usergrp)
        '    GetUserGroup = dsusergrp
        'End Function
        'Public Function AddUser(ByVal User As User) As Boolean
        '    Dim ctx As Web.HttpContext = Web.HttpContext.Current
        '    Dim strError As String
        '    '// make sure the user's name begins with an alphabetic character
        '    If Not Regex.IsMatch(User.Password, "^[A-Za-z]\w{5,10}$") Then
        '        strError = "Invalid Password<BR>"
        '    End If

        '    '// make sure the user's name begins with an alphabetic character
        '    If Not Regex.IsMatch(User.Name, "^[A-Za-z].*") Then
        '        strError = strError & "Invalid User Name<BR>"
        '    End If
        '    'lsSql.Append("SEELCT '*' FROM USER_MSTR WHERE UM_USER_ID='")
        '    'lsSql.Append(Common.Parse(User.UserID)).Append("' AND UM_COMPANY_ID='")
        '    'lsSql.Append(Common.Parse(User.CompanyID)).Append("'")
        '    lsSql = "SELECT '*' FROM USERS_MSTR WHERE UM_USER_ID='" & _
        '    Common.Parse(User.UserID) & "' AND UM_COMPANY_ID='" & _
        '    Common.Parse(User.CompanyID) & "'"

        '    Try
        '        If objDb.Exist(lsSql) Then
        '            'Throw New CustomException(User.UserID & " is alrAppCommony being used")
        '            strError = strError & User.UserID & " is alrAppCommony being used<BR>"
        '            Throw New CustomException(strError)
        '        Else
        '            lsSql = "INSERT INTO USERS_MSTR(UM_USER_ID,UM_USER_NAME,UM_COMPANY_ID," & _
        '            "UM_DEPT_ID,UM_PWD,UM_PWD_EXP,UM_EMAIL,UM_PHONE_NO,UM_FAX_NO," & _
        '            "UM_TITLE,UM_APP_LIMIT,UM_ENT_BY,UM_ENT_DATE,UM_MOD_BY,UM_MOD_DATE," & _
        '            "UM_STATUS,UM_LOGIN_FAILED_COUNTER,UM_FIRSTLOGON_FLAG,UM_LAST_LOGON)" & _
        '            "VALUES('" & _
        '            Common.Parse(User.UserID) & "','" & Common.Parse(User.Name) & "','" & _
        '            Common.Parse(User.CompanyID) & "','" & Common.Parse(User.DeptID) & "',CAST('" & _
        '            Common.Parse(User.Password) & "' AS VARBINARY(50)),'" & Common.Parse(User.PwdExpDate) & "','" & _
        '            Common.Parse(User.Email) & "','" & Common.Parse(User.PhoneNo) & "','" & _
        '            Common.Parse(User.FaxNo) & "','" & Common.Parse(User.Title) & "'," & _
        '            Common.Parse(User.ApproveLimit) & ",'" & Common.Parse(ctx.Session("UserID")) & "',GETDATE(),'" & _
        '            Common.Parse(ctx.Session("UserID")) & "',GETDATE(),'A',0,'Y',NULL)"
        '            objDb.Execute(lsSql)
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        'Name       : AddUser
        'Author     : kk
        'Descption  : Add new user
        'LastUpadte : 22 Nox 2004
        'Public Function AddUser(ByVal pUser As User, ByVal pHubLevel As Boolean, ByVal pAppPackageId As String) As Boolean
        '    AddUser = False
        '    Dim strSQLDup, strSQLDel, strSQLUserLimit As String
        '    Dim Query(0) As String
        '    Dim strCompid As String

        '    If pHubLevel Then
        '        strCompid = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompid = ctx.Session("CompanyId")
        '    End If

        '    'SQL to check whether user duplicated
        '    strSQLDup = "SELECT * FROM USER_MSTR WHERE UM_DELETED IN ('N','S')  AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyID) & "'"

        '    'SQL to check whether user is used previously
        '    strSQLDel = "SELECT * FROM USER_MSTR WHERE UM_DELETED='Y' AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyID) & "'"

        '    Try
        '        If objDb.Exist(strSQLDup) Then
        '            strUserMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSQLDel) Then
        '            strUserMassage = Common.RecordUsed
        '            Return False
        '        Else
        '            If pHubLevel Then
        '                If IsExceedAdminLimit(strCompid) Then
        '                    Return False
        '                End If
        '            Else
        '                If IsExceedUserLimit(strCompid) Then
        '                    '*****************meilai 12/1/2005**************************************
        '                    'strUserMassage = "User limit exceeded."
        '                    strUserMassage = "The number of active users exceeds the User License."
        '                    '***********************************************************************
        '                    Return False
        '                End If
        '                If IsExceedReportUser(strCompid, pUser) Then Return False
        '            End If
        '        End If

        '        'Modified by Joon 
        '        lsSql = "INSERT INTO USER_MSTR(" &
        '                "UM_USER_ID,UM_PASSWORD,UM_DELETED," &
        '                "UM_USER_NAME,UM_COY_ID,UM_DEPT_ID," &
        '                "UM_EMAIL,UM_TEL_NO,UM_FAX_NO," &
        '                "UM_APP_LIMIT,UM_PO_APP_LIMIT,UM_INVOICE_APP_LIMIT,UM_DESIGNATION," &
        '                "UM_USER_SUSPEND_IND,UM_PASSWORD_LAST_CHANGED,UM_NEW_PASSWORD_IND,UM_STATUS," &
        '                "UM_NEXT_EXPIRE_DT,UM_LAST_LOGIN_DT,UM_QUESTION," &
        '                "UM_ANSWER,UM_ENT_BY,UM_ENT_DATE,UM_RECORD_COUNT,UM_EMAIL_CC, UM_MASS_APP, UM_MRS_MASS_APP, " &
        '                "UM_STK_TYPE_SPOT,UM_STK_TYPE_STOCK,UM_STK_TYPE_MRO) " &
        '                "VALUES('" &
        '                Common.Parse(pUser.UserID) & "', CAST('" & Common.EncryptString(pUser.Password) & "' AS BINARY),'N','" &
        '                Common.Parse(pUser.Name) & "','" & Common.Parse(strCompid) & "','" & Common.Parse(pUser.DeptID) & "','" &
        '                Common.Parse(pUser.Email) & "','" & Common.Parse(pUser.PhoneNo) & "','" & Common.Parse(pUser.FaxNo) & "'," &
        '                IIf(IsNothing(pUser.ApproveLimit), 0, Common.ConvertMoney(pUser.ApproveLimit)) & "," &
        '                IIf(IsNothing(pUser.POApproveLimit), 0, Common.ConvertMoney(pUser.POApproveLimit)) & "," &
        '                 IIf(IsNothing(pUser.InvoiceApproveLimit), 0, Common.ConvertMoney(pUser.InvoiceApproveLimit)) & ",'" &
        '                Common.Parse(pUser.Designation) & "'," &
        '                "0,NULL,'Y','" & Common.Parse(pUser.Status) & "'," &
        '                Common.ConvertDate(pUser.NextExpiredDt) & ",NULL,NULL," &
        '                "NULL,'" & ctx.Session("UserID") & "'," & Common.ConvertDate(Now) & ",10,'','" & Common.Parse(pUser.MassApp) & "','" & Common.Parse(pUser.MrsMassApp) & "','" &
        '                Common.Parse(pUser.StockTypeSpot) & "','" & Common.Parse(pUser.StockTypeStock) & "','" & Common.Parse(pUser.StockTypeMro) & "')"
        '        Common.Insert2Ary(Query, lsSql)

        '        'Added by Joon on 22 June 2011
        '        'If Fixed role = Purchasing Officer
        '        If pUser.strFixedRole = Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") Then
        '            Dim strSql As String
        '            Dim dt1 As New DataTable
        '            Dim i As Integer

        '            'strSql = "SELECT CT_ID,CT_NAME,CT_PARENT_ID FROM commodity_type WHERE CT_LAST_LVL=1"
        '            strSql = "SELECT CT_ROOT_PREFIX FROM commodity_type WHERE CT_PARENT_ID IS NULL"
        '            dt1 = objDb.FillDt(strSql)
        '            For i = 0 To dt1.Rows.Count - 1
        '                strSql = "SELECT '*' " _
        '                     & "FROM user_assign " _
        '                     & "WHERE UA_COY_ID='" & Common.Parse(pUser.CompanyID) & "' " _
        '                     & "AND UA_USER_ID='" & Common.Parse(pUser.UserID) & "' " _
        '                     & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
        '                     & "AND UA_ASSIGN_VALUE='" & dt1.Rows(i).Item("CT_ROOT_PREFIX") & "'"
        '                If objDb.Exist(strSql) = 0 Then
        '                    lsSql = "INSERT INTO user_assign(" &
        '                           "UA_COY_ID,UA_USER_ID,UA_FIXED_ROLE,UA_TYPE,UA_ASSIGN_VALUE) " &
        '                           "VALUES('" &
        '                           Common.Parse(pUser.CompanyID) & "','" & Common.Parse(pUser.UserID) & "', '" &
        '                           Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "','CT','" &
        '                           dt1.Rows(i).Item("CT_ROOT_PREFIX") & "')"
        '                    Common.Insert2Ary(Query, lsSql)
        '                End If
        '            Next
        '        End If

        '        UpdateUsers_UsrGrp(Query, pUser, pAppPackageId)

        '        If pAppPackageId = EnumAppPackage.eProcure.ToString Then
        '            ' for fixedrole=buyer/PO/PM
        '            InsertUsers_Addr(Query, pUser)
        '        End If

        '        If objDb.BatchExecute(Query) Then
        '            strUserMassage = Common.RecordSave
        '            Return True
        '        End If

        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        Public Function GetUserCount(ByVal pCompId As String, Optional ByVal pCountSuperAdmin As Boolean = False, Optional ByVal pActiveOnly As Boolean = False) As Integer
            Dim iUserLicenses, iCurrUser As Integer
            Dim strSQL As String
            'SQL to check user limit
            strSQL = "SELECT COUNT(*) FROM USER_MSTR WHERE UM_DELETED <> 'Y' " &
                     "AND UM_COY_ID = '" & Common.Parse(pCompId) & "'"

            If pActiveOnly Then
                strSQL &= " AND UM_STATUS = 'A' "
            End If

            If Not pCountSuperAdmin Then
                strSQL &= " AND UM_USER_ID NOT IN (" &
                        " SELECT UU_USER_ID FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
                        " UU_USRGRP_ID = UGM_USRGRP_ID" &
                        " AND UU_APP_PKG = UGM_APP_PKG" &
                        " AND UU_COY_ID = UM_COY_ID" &
                        " AND UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "'" &
                        ")"
            End If

            iCurrUser = objDb.GetVal(strSQL)
            Return iCurrUser

        End Function

        Public Function IsExceedUserLimit(ByVal pCompId As String) As Boolean
            Dim objDcom As New EAD.DBCom

            Dim iUserLicenses, iCurrUser As Integer
            Dim strSQL As String
            'SQL to check user limit
            strSQL = "SELECT COUNT(*) As Count FROM USER_MSTR WHERE " &
                " UM_DELETED<>'Y' AND UM_STATUS='A' " &
                " AND UM_COY_ID='" & Common.Parse(pCompId) & "'" &
                " AND UM_USER_ID NOT IN (" &
                " SELECT UU_USER_ID FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
                " UU_USRGRP_ID = UGM_USRGRP_ID" &
                " AND UU_APP_PKG = UGM_APP_PKG" &
                " AND UU_COY_ID = UM_COY_ID" &
                " AND UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "'" &
                ")"

            'iCurrUser = Convert.ToInt64(objDb.GetVal(strSQL))
            Dim dt As DataTable = objDcom.FillDt(strSQL)
            iCurrUser = If(dt.Rows.Count > 0, CStr(dt.Rows(0).Item("Count")), 0)

            strSQL = "SELECT CM_LICENSE_USERS FROM COMPANY_MSTR " &
                     " WHERE CM_COY_ID='" & Common.Parse(pCompId) & "' AND CM_DELETED<>'Y'"

            'iUserLicenses = Convert.ToInt64(objDb.GetVal(strSQL))
            dt = objDcom.FillDt(strSQL)
            iUserLicenses = If(dt.Rows.Count > 0, CStr(dt.Rows(0).Item("CM_LICENSE_USERS")), 0)

            If iCurrUser >= iUserLicenses Then
                strUserMassage = "The number of active users exceeds the User License."
                Return True
            Else
                Return False
            End If

        End Function

        Public Function IsExceedReportUser(ByVal pCompId As String, ByVal pUser As User) As Boolean
            Dim strSQL As String

            ' Check if Report Administrator Role is selected
            If pUser.UserGroup = "" Then Return False

            Dim i As Integer
            Dim spUsrGrp As Array = Split(pUser.UserGroup, ",")
            Dim strUserGroupList As String = ""

            For i = 0 To spUsrGrp.Length - 1
                strUserGroupList &= ",'" & spUsrGrp(i) & "'"
            Next
            strUserGroupList = strUserGroupList.Substring(1)

            strSQL = " SELECT '1' FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
            " UU_USRGRP_ID = UGM_USRGRP_ID" &
            " AND UU_APP_PKG = UGM_APP_PKG" &
            " AND UGM_FIXED_ROLE = '" & FixedRole.Report_Administrator.ToString.Replace("_", " ") & "'" &
            " AND UU_COY_ID = '" & Common.Parse(pCompId) & "'" &
            " AND UU_USRGRP_ID IN (" & strUserGroupList & ")"

            If objDb.GetVal(strSQL) = "" Then Return False

            ' Check if user alrAppCommony exists
            strSQL = "SELECT '1' FROM USER_MSTR WHERE " &
                " UM_USER_ID = '" & Common.Parse(pUser.UserID) & "'"

            If objDb.GetVal(strSQL) = "" Then
                Return IsExceedReportUserS(pCompId)
            Else
                Return IsExceedReportUserS(pCompId, pUser.UserID)
            End If
        End Function

        'Public Function IsExceedReportUser(ByVal pCompId As String, ByVal pUserId As String) As Boolean
        '    Dim strSQL As String

        '    strSQL = "SELECT '1' FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
        '        " UU_USRGRP_ID = UGM_USRGRP_ID" &
        '        " AND UU_APP_PKG = UGM_APP_PKG" &
        '        " AND UGM_FIXED_ROLE = '" & FixedRole.Report_Administrator.ToString.Replace("_", " ") & "'" &
        '        " AND UU_COY_ID = '" & Common.Parse(pCompId) & "'" &
        '        " AND UU_USER_ID = '" & Common.Parse(pUserId) & "'"

        '    If objDb.GetVal(strSQL) = "" Then Return False

        '    Return IsExceedReportUserS(pCompId, pUserId)
        'End Function

        Private Function IsExceedReportUserS(ByVal pCompId As String, Optional ByVal pUserId As String = Nothing) As Boolean
            Dim strSQL As String
            Dim blnReportUserRole As Boolean = False
            Dim iReportUser, iCurrReportUser As Integer

            strSQL = "SELECT COUNT(*) FROM USER_MSTR WHERE " &
                " UM_DELETED<>'Y' AND UM_STATUS='A' " &
                " AND UM_COY_ID='" & Common.Parse(pCompId) & "'" &
                " AND UM_USER_ID IN (" &
                " SELECT UU_USER_ID FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
                " UU_USRGRP_ID = UGM_USRGRP_ID" &
                " AND UU_APP_PKG = UGM_APP_PKG" &
                " AND UU_COY_ID = UM_COY_ID" &
                " AND UGM_FIXED_ROLE = '" & FixedRole.Report_Administrator.ToString.Replace("_", " ") & "'" &
                ")"

            If Not pUserId Is Nothing Then strSQL &= " AND UM_USER_ID <> '" & Common.Parse(pUserId) & "'"

            iCurrReportUser = objDb.GetVal(strSQL)

            strSQL = "SELECT ISNULL(CM_REPORT_USERS, 0) AS REPORT_USERS FROM COMPANY_MSTR " &
                     " WHERE CM_COY_ID='" & Common.Parse(pCompId) & "' AND CM_DELETED<>'Y'"
            iReportUser = objDb.GetVal(strSQL)

            If iCurrReportUser >= iReportUser Then
                strUserMassage = "The number of active report users exceeded the Report User License."
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsExceedAdminLimit(ByVal pCompId As String, Optional ByVal pUserId As String = Nothing) As Boolean
            Dim iUserLicenses, iCurrUser As Integer
            Dim strSQL As String
            'SQL to check admin user limit
            strSQL = "SELECT COUNT(*) FROM USER_MSTR WHERE " &
                " UM_DELETED<>'Y' AND UM_STATUS='A' " &
                " AND UM_COY_ID='" & Common.Parse(pCompId) & "'" &
                " AND UM_USER_ID IN (" &
                " SELECT UU_USER_ID FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
                " UU_USRGRP_ID = UGM_USRGRP_ID" &
                " AND UU_APP_PKG = UGM_APP_PKG" &
                " AND UU_COY_ID = UM_COY_ID" &
                " AND UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "'" &
                ")"

            If Not pUserId Is Nothing Then
                strSQL &= " AND UM_USER_ID <> '" & pUserId & "'"
            End If

            iCurrUser = objDb.GetVal(strSQL)

            'strSQL = "SELECT COUNT(*) FROM USER_MSTR, USERS_USRGRP " & _
            '         "WHERE USER_MSTR.UM_COY_ID = USERS_USRGRP.UU_COY_ID " & _
            '         "AND USER_MSTR.UM_USER_ID = USERS_USRGRP.UU_USER_ID " & _
            '         "AND USERS_USRGRP.UU_APP_PKG = '" & EnumAppPackage.All.ToString & "' " & _
            '         "AND UM_DELETED <>'Y' AND UM_STATUS='A' " & _
            '         "AND UM_COY_ID='" & Common.Parse(pCompId) & "'"
            'iCurrUser = objDb.GetVal(strSQL)

            iUserLicenses = 1

            If iCurrUser >= iUserLicenses Then
                strUserMassage = "Only one active admin user is allowed."
                Return True
            Else
                Return False
            End If

        End Function

        'Name       : UpdateUsr_UsrGroup
        'Author     : kk
        'Descption  : Update User_userGroup
        'LastUpadte : 22 Nov 2004
        Private Sub UpdateUsers_UsrGrp(ByVal userID As String, ByRef pQuery() As String, ByVal pUser As User, ByVal pAppPackageId As String)
            'Delete old user user group
            'Michelle (4/5/2012) - Issue 1604 (Super Admin group will not be deleted)
            'lsSql = "DELETE FROM USERS_USRGRP WHERE UU_USER_ID ='" & Common.Parse(pUser.UserID) & "' AND UU_COY_ID='" & Common.Parse(pUser.CompanyID) & "' AND UU_APP_PKG='" & pAppPackageId & "'"
            lsSql = "DELETE FROM USERS_USRGRP WHERE UU_USER_ID ='" & Common.Parse(pUser.UserID) & "' AND UU_COY_ID='" & Common.Parse(pUser.CompanyID) & "' AND UU_APP_PKG='" & pAppPackageId & "' "
            lsSql &= " AND UU_USRGRP_ID IN (SELECT DISTINCT UGM_USRGRP_ID FROM USER_GROUP_MSTR WHERE UGM_FIXED_ROLE <> 'SUPER ADMIN') "
            Common.Insert2Ary(pQuery, lsSql)

            If pUser.UserGroup <> "" Then
                Dim i As Integer
                Dim spUsrGrp As Array
                spUsrGrp = Split(pUser.UserGroup, ",")

                'add new user user group
                For i = 0 To spUsrGrp.Length - 1
                    lsSql = "INSERT INTO USERS_USRGRP (UU_USRGRP_ID,UU_COY_ID,UU_USER_ID,UU_MOD_BY,UU_APP_PKG) VALUES('" & spUsrGrp(i) & "','" & Common.Parse(pUser.CompanyID) & "','" &
                        Common.Parse(pUser.UserID) & "','" & userID & "','" & pAppPackageId & "')"
                    Common.Insert2Ary(pQuery, lsSql)
                Next
            End If
        End Sub

        'Private Sub UpdateUsers_UsrGrp(ByRef pQuery() As String, ByVal pUserUsrGrp As UserUsrGrp)

        '    'Delete existing user user group
        '    'Michelle (3/6/2011) - for Issue 540
        '    If LCase(Common.Parse(pUserUsrGrp.CompanyId)) <> "hub" Then
        '        lsSql = "DELETE FROM USERS_USRGRP WHERE " &
        '            " UU_USER_ID = '" & Common.Parse(pUserUsrGrp.UserDetail.UserID) & "' " &
        '            " AND UU_COY_ID = '" & Common.Parse(pUserUsrGrp.CompanyId) & "'"

        '        Common.Insert2Ary(pQuery, lsSql)
        '    End If

        '    Dim strAppPackageId As String
        '    Dim strUserGroupId As String
        '    Dim i As Integer

        '    ' Add new user's user group
        '    For i = 0 To pUserUsrGrp.PackageList.Count - 1
        '        strAppPackageId = pUserUsrGrp.PackageList(i).ToString
        '        strUserGroupId = pUserUsrGrp.UserGroupList(i).ToString

        '        'For Each strAppPackageId In pUserUsrGrp.PackageList
        '        '    For Each strUserGroupId In pUserUsrGrp.UserGroupList
        '        lsSql = "INSERT INTO USERS_USRGRP (UU_USRGRP_ID, UU_COY_ID, UU_USER_ID, UU_MOD_BY, UU_APP_PKG" &
        '            ") VALUES (" &
        '            " '" & Common.Parse(strUserGroupId) & "'," &
        '            " '" & Common.Parse(pUserUsrGrp.CompanyId) & "'," &
        '            " '" & Common.Parse(pUserUsrGrp.UserDetail.UserID) & "'," &
        '            " '" & ctx.Session("UserID") & "'," &
        '            " '" & strAppPackageId & "')"

        '        Common.Insert2Ary(pQuery, lsSql)
        '        '    Next
        '        'Next
        '    Next
        'End Sub
        'Private Sub UpdateUsers_UsrGrpByBilling(ByRef pQuery() As String, ByVal pUser As User, ByVal pAppPackageId As String, ByVal UserID As String)

        '    'Delete existing user user group
        '    'Michelle (3/6/2011) - for Issue 540
        '    lsSql = "DELETE FROM USERS_USRGRP WHERE UU_USER_ID ='" & Common.Parse(pUser.UserID) & "' AND UU_COY_ID='" & Common.Parse(pUser.CompanyID) & "' AND UU_APP_PKG='" & pAppPackageId & "' "
        '    lsSql &= " AND UU_USRGRP_ID IN (SELECT DISTINCT UGM_USRGRP_ID FROM USER_GROUP_MSTR WHERE UGM_FIXED_ROLE <> 'SUPER ADMIN') "
        '    Common.Insert2Ary(pQuery, lsSql)

        '    Dim i As Integer

        '    If pUser.UserGroup <> "" Then
        '        Dim spUsrGrp As Array
        '        spUsrGrp = Split(pUser.UserGroup, ",")

        '        'add new user user group
        '        For i = 0 To spUsrGrp.Length - 1
        '            lsSql = "INSERT INTO USERS_USRGRP (UU_USRGRP_ID,UU_COY_ID,UU_USER_ID,UU_MOD_BY,UU_APP_PKG) VALUES('" & spUsrGrp(i) & "','" & Common.Parse(pUser.CompanyID) & "','" &
        '                Common.Parse(pUser.UserID) & "','" & UserID & "','" & pAppPackageId & "')"
        '            Common.Insert2Ary(pQuery, lsSql)
        '        Next
        '    End If
        'End Sub

        'Public Function AddUser(ByVal pUser As UserUsrGrp, ByVal pHubLevel As Boolean) As Boolean
        '    AddUser = False

        '    Dim strSQLDup, strSQLDel, strSQLUserLimit As String
        '    Dim Query(0) As String
        '    Dim strCompid As String
        '    Dim strAppPackageId As String

        '    If pHubLevel Then
        '        strCompid = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompid = ctx.Session("CompanyId")

        '        If Not pUser.PackageList Is Nothing Then
        '            If pUser.PackageList.Count > 0 Then
        '                strAppPackageId = pUser.PackageList.Item(0).ToString
        '            End If
        '        End If
        '    End If

        '    'SQL to check whether user duplicated
        '    strSQLDup = "SELECT * FROM USER_MSTR WHERE UM_DELETED IN ('N','S')  AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserDetail.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyId) & "'"

        '    'SQL to check whether user is used previously
        '    strSQLDel = "SELECT * FROM USER_MSTR WHERE UM_DELETED='Y' AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserDetail.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyId) & "'"

        '    Try
        '        If objDb.Exist(strSQLDup) Then
        '            strUserMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSQLDel) Then
        '            strUserMassage = Common.RecordUsed
        '            Return False
        '        Else
        '            If pHubLevel Then
        '                If IsExceedAdminLimit(strCompid) Then
        '                    Return False
        '                End If
        '            Else
        '                If IsExceedUserLimit(strCompid) Then
        '                    '*****************meilai 12/1/2005**************************************
        '                    'strUserMassage = "User limit exceeded."
        '                    strUserMassage = "The number of active users exceeds the User License."
        '                    '***********************************************************************
        '                    Return False
        '                End If
        '                If IsExceedReportUser(strCompid, pUser.UserDetail) Then Return False
        '            End If
        '        End If

        '        'Modified by Joon on 21st Sept 2010
        '        lsSql = "INSERT INTO USER_MSTR(" &
        '                "UM_USER_ID,UM_PASSWORD,UM_DELETED," &
        '                "UM_USER_NAME,UM_COY_ID,UM_DEPT_ID," &
        '                "UM_EMAIL,UM_TEL_NO,UM_FAX_NO," &
        '                "UM_APP_LIMIT,UM_INVOICE_APP_LIMIT,UM_DESIGNATION," &
        '                "UM_USER_SUSPEND_IND,UM_PASSWORD_LAST_CHANGED,UM_NEW_PASSWORD_IND,UM_STATUS," &
        '                "UM_NEXT_EXPIRE_DT,UM_LAST_LOGIN_DT,UM_QUESTION," &
        '                "UM_ANSWER,UM_ENT_BY,UM_ENT_DATE,UM_RECORD_COUNT,UM_EMAIL_CC) " &
        '                "VALUES('" &
        '                Common.Parse(pUser.UserDetail.UserID) & "', CAST('" & Common.EncryptString(pUser.UserDetail.Password) & "' AS BINARY),'N','" &
        '                Common.Parse(pUser.UserDetail.Name) & "','" & Common.Parse(strCompid) & "','" & Common.Parse(pUser.UserDetail.DeptID) & "','" &
        '                Common.Parse(pUser.UserDetail.Email) & "','" & Common.Parse(pUser.UserDetail.PhoneNo) & "','" & Common.Parse(pUser.UserDetail.FaxNo) & "'," &
        '                IIf(IsNothing(pUser.UserDetail.ApproveLimit), 0, Common.ConvertMoney(pUser.UserDetail.ApproveLimit)) & "," &
        '                 IIf(IsNothing(pUser.UserDetail.InvoiceApproveLimit), 0, Common.ConvertMoney(pUser.UserDetail.InvoiceApproveLimit)) & ",'" &
        '                 Common.Parse(pUser.UserDetail.Designation) & "'," &
        '                "0,NULL,'Y','" & Common.Parse(pUser.UserDetail.Status) & "'," &
        '                Common.ConvertDate(pUser.UserDetail.NextExpiredDt) & ",NULL,NULL," &
        '                "NULL,'" & ctx.Session("UserID") & "'," & Common.ConvertDate(Now) & ",10,'')"

        '        Common.Insert2Ary(Query, lsSql)

        '        If pHubLevel Then
        '            UpdateUsers_UsrGrp(Query, pUser)
        '        Else
        '            UpdateUsers_UsrGrp(Query, pUser.UserDetail, strAppPackageId)
        '        End If

        '        If strAppPackageId = EnumAppPackage.eProcure.ToString Then
        '            ' for fixedrole=buyer/PO/PM
        '            InsertUsers_Addr(Query, pUser.UserDetail)
        '        End If

        '        If objDb.BatchExecute(Query) Then
        '            strUserMassage = Common.RecordSave
        '            Return True
        '        End If

        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function
        'Public Function AddUserByBilling(ByVal pUser As User, ByVal pHubLevel As Boolean, ByVal pAppPackageId As String, ByVal UserID As String) As Boolean
        '    AddUserByBilling = False

        '    Dim strSQLDup, strSQLDel As String
        '    Dim Query(0) As String
        '    Dim strCompid As String
        '    strCompid = pUser.CompanyID


        '    'SQL to check whether user duplicated
        '    strSQLDup = "SELECT * FROM USER_MSTR WHERE UM_DELETED IN ('N','S')  AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyID) & "'"

        '    'SQL to check whether user is used previously
        '    strSQLDel = "SELECT * FROM USER_MSTR WHERE UM_DELETED='Y' AND UM_USER_ID='" &
        '    Common.Parse(pUser.UserID) & "' AND UM_COY_ID='" &
        '    Common.Parse(pUser.CompanyID) & "'"

        '    Try
        '        If objDb.Exist(strSQLDup) Then
        '            strUserMassage = Common.RecordDuplicate
        '            Return False
        '        ElseIf objDb.Exist(strSQLDel) Then
        '            strUserMassage = Common.RecordUsed
        '            Return False
        '        Else
        '            If IsExceedUserLimit(strCompid) Then
        '                '*****************meilai 12/1/2005**************************************
        '                'strUserMassage = "User limit exceeded."
        '                strUserMassage = "The number of active users exceeds the User License."
        '                '***********************************************************************
        '                Return False
        '            End If
        '            If IsExceedReportUser(strCompid, pUser) Then Return False
        '        End If

        '        'Modified by Joon on 21st Sept 2010
        '        lsSql = "INSERT INTO USER_MSTR(" &
        '                "UM_USER_ID,UM_PASSWORD,UM_DELETED," &
        '                "UM_USER_NAME,UM_COY_ID,UM_DEPT_ID," &
        '                "UM_EMAIL,UM_TEL_NO,UM_FAX_NO," &
        '                "UM_APP_LIMIT,UM_INVOICE_APP_LIMIT,UM_DESIGNATION," &
        '                "UM_USER_SUSPEND_IND,UM_PASSWORD_LAST_CHANGED,UM_NEW_PASSWORD_IND,UM_STATUS," &
        '                "UM_NEXT_EXPIRE_DT,UM_LAST_LOGIN_DT,UM_QUESTION," &
        '                "UM_ANSWER,UM_ENT_BY,UM_ENT_DATE,UM_RECORD_COUNT,UM_EMAIL_CC) " &
        '                "VALUES('" &
        '                Common.Parse(pUser.UserID) & "', CAST('" & Common.EncryptString(pUser.Password) & "' AS BINARY),'N','" &
        '                Common.Parse(pUser.Name) & "','" & Common.Parse(strCompid) & "','" & Common.Parse(pUser.DeptID) & "','" &
        '                Common.Parse(pUser.Email) & "','" & Common.Parse(pUser.PhoneNo) & "','" & Common.Parse(pUser.FaxNo) & "'," &
        '                IIf(IsNothing(pUser.ApproveLimit), 0, Common.ConvertMoney(pUser.ApproveLimit)) & "," &
        '                 IIf(IsNothing(pUser.InvoiceApproveLimit), 0, Common.ConvertMoney(pUser.InvoiceApproveLimit)) & ",'" &
        '                 Common.Parse(pUser.Designation) & "'," &
        '                "0,NULL,'" & pUser.NewPwdInd & "','" & Common.Parse(pUser.Status) & "'," &
        '                Common.ConvertDate(pUser.NextExpiredDt) & ",NULL,NULL," &
        '                "NULL,'" & UserID & "'," & Common.ConvertDate(Now) & ",10,'')"

        '        Common.Insert2Ary(Query, lsSql)
        '        'If Fixed role = Purchasing Officer
        '        If pUser.strFixedRole = Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") Then
        '            Dim strSql As String
        '            Dim dt1 As New DataTable
        '            Dim i As Integer

        '            strSql = "SELECT CT_ROOT_PREFIX FROM commodity_type WHERE CT_PARENT_ID IS NULL"
        '            dt1 = objDb.FillDt(strSql)
        '            For i = 0 To dt1.Rows.Count - 1
        '                strSql = "SELECT '*' " _
        '                     & "FROM user_assign " _
        '                     & "WHERE UA_COY_ID='" & Common.Parse(pUser.CompanyID) & "' " _
        '                     & "AND UA_USER_ID='" & Common.Parse(pUser.UserID) & "' " _
        '                     & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
        '                     & "AND UA_ASSIGN_VALUE='" & dt1.Rows(i).Item("CT_ROOT_PREFIX") & "'"
        '                If objDb.Exist(strSql) = 0 Then
        '                    lsSql = "INSERT INTO user_assign(" &
        '                           "UA_COY_ID,UA_USER_ID,UA_FIXED_ROLE,UA_TYPE,UA_ASSIGN_VALUE) " &
        '                           "VALUES('" &
        '                           Common.Parse(pUser.CompanyID) & "','" & Common.Parse(pUser.UserID) & "', '" &
        '                           Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "','CT','" &
        '                           dt1.Rows(i).Item("CT_ROOT_PREFIX") & "')"
        '                    Common.Insert2Ary(Query, lsSql)
        '                End If
        '            Next
        '        End If

        '        UpdateUsers_UsrGrpByBilling(Query, pUser, pAppPackageId, UserID)

        '        If pAppPackageId = EnumAppPackage.eProcure.ToString Then
        '            ' for fixedrole=buyer/PO/PM
        '            InsertUsers_Addr(Query, pUser)
        '        End If

        '        If objDb.BatchExecute(Query) Then
        '            strUserMassage = Common.RecordSave
        '            Return True
        '        End If

        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        'Public Function UpdateUser(ByVal pUser As UserUsrGrp, ByVal pHubLevel As Boolean, Optional ByVal pChkUserLimit As Boolean = True) As Boolean
        '    UpdateUser = False
        '    Dim Query(0) As String
        '    Dim strAppPackageId As String

        '    Try
        '        'kk.remark.18/01/2004 pChkUserLimit=False mean not need chk for user limit. eg- when user update their own details
        '        'but in update user activation needed to chk
        '        If pChkUserLimit And pUser.UserDetail.Status = "A" Then
        '            If pHubLevel Then
        '                If IsExceedAdminLimit(pUser.CompanyId, pUser.UserDetail.UserID) Then
        '                    Return False
        '                End If
        '            Else
        '                If IsExceedUserLimit(pUser.CompanyId) Then
        '                    Return False
        '                End If
        '            End If
        '        End If

        '        'This os to cater if the user update status, if chg from I -> A will required to chk
        '        'user limit
        '        Dim strSQL, strStatus As String
        '        strSQL = "SELECT UM_STATUS FROM USER_MSTR WHERE UM_USER_ID='" & Common.Parse(pUser.UserDetail.UserID) &
        '                   "' AND UM_COY_ID='" & Common.Parse(pUser.CompanyId) & "' AND UM_DELETED<>'Y'"
        '        strStatus = objDb.GetVal(strSQL)
        '        If UCase(strStatus) = "I" And pUser.UserDetail.Status = "A" Then
        '            If pHubLevel Then
        '                If IsExceedAdminLimit(pUser.CompanyId, pUser.UserDetail.UserID) Then
        '                    Return False
        '                End If
        '            Else
        '                If IsExceedUserLimit(pUser.CompanyId) Then
        '                    Return False
        '                End If
        '            End If
        '        End If

        '        If IsExceedReportUser(pUser.CompanyId, pUser.UserDetail) And pUser.UserDetail.Status = "A" Then Return False

        '        If pHubLevel Then
        '            lsSql = "UPDATE USER_MSTR SET UM_USER_NAME='" & Common.Parse(pUser.UserDetail.Name) & "'," &
        '                    "UM_EMAIL='" & Common.Parse(pUser.UserDetail.Email) & "'," &
        '                    "UM_STATUS='" & Common.Parse(pUser.UserDetail.Status) & "'," &
        '                    "UM_MOD_BY='" & ctx.Session("UserID") & "'," &
        '                    "UM_MOD_DT=GETDATE()," &
        '                    "UM_DELETED='" & Common.Parse(pUser.UserDetail.DeleteInd) & "' " &
        '                    "WHERE UM_USER_ID='" & pUser.UserDetail.UserID & "' AND UM_COY_ID='" & ctx.Session("CompanyIdToken") & "'"

        '            Common.Insert2Ary(Query, lsSql)

        '            UpdateUsers_UsrGrp(Query, pUser)
        '        Else
        '            If Not pUser.PackageList Is Nothing Then
        '                If pUser.PackageList.Count > 0 Then
        '                    strAppPackageId = pUser.PackageList.Item(0).ToString
        '                End If
        '            End If

        '            lsSql = "UPDATE USER_MSTR SET UM_USER_NAME='" & Common.Parse(pUser.UserDetail.Name) & "'," &
        '                    "UM_DEPT_ID='" & Common.Parse(pUser.UserDetail.DeptID) & "'," &
        '                    "UM_EMAIL='" & Common.Parse(pUser.UserDetail.Email) & "'," &
        '                    "UM_TEL_NO='" & Common.Parse(pUser.UserDetail.PhoneNo) & "'," &
        '                    "UM_FAX_NO='" & Common.Parse(pUser.UserDetail.FaxNo) & "'," &
        '                    "UM_DESIGNATION='" & Common.Parse(pUser.UserDetail.Designation) & "'," &
        '                    "UM_APP_LIMIT=" & Common.ConvertMoney(pUser.UserDetail.ApproveLimit) & "," &
        '                    "UM_INVOICE_APP_LIMIT=" & Common.ConvertMoney(pUser.UserDetail.InvoiceApproveLimit) & "," &
        '                    "UM_STATUS='" & Common.Parse(pUser.UserDetail.Status) & "'," &
        '                    "UM_MOD_BY='" & ctx.Session("UserID") & "'," &
        '                    "UM_MOD_DT=GETDATE()," &
        '                    "UM_RECORD_COUNT = " & pUser.UserDetail.PageCount & ", " &
        '                    "UM_EMAIL_CC = '" & Common.Parse(pUser.UserDetail.CCList) & "', " &
        '                    "UM_DELETED='" & Common.Parse(pUser.UserDetail.DeleteInd) & "' " &
        '                    "WHERE UM_USER_ID='" & pUser.UserDetail.UserID & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '            Common.Insert2Ary(Query, lsSql)

        '            UpdateUsers_UsrGrp(Query, pUser.UserDetail, strAppPackageId)
        '        End If

        '        If objDb.BatchExecute(Query) Then
        '            strUserMassage = Common.RecordSave
        '            Return True
        '        End If

        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        ''Name       : InsertUsers_Addr
        ''Author     : ai chu
        ''Descption  : Insert default address into USERS_ADDR
        ''LastUpdate : 01 Mac 2005
        'Private Sub InsertUsers_Addr(ByRef pQuery() As String, ByVal pUser As User)
        '    If pUser.UserGroup <> "" Then
        '        Dim i As Integer
        '        Dim spUsrGrp As Array
        '        spUsrGrp = Split(pUser.UserGroup, ",")

        '        ' ai chu modified on 30/09/2005
        '        ' if user select same role more than 1 (apply to Buyer, PO, PM, SK, 2nd)
        '        ' only insert once
        '        Dim blnBuyer As Boolean = True
        '        Dim blnSK As Boolean = True
        '        Dim bln2nd As Boolean = True
        '        Dim blnInsert As Boolean = False

        '        'add new user user group
        '        For i = 0 To spUsrGrp.Length - 1
        '            If blnBuyer And IsRoleSelected(spUsrGrp(i), FixedRole.Buyer) Or IsRoleSelected(spUsrGrp(i), FixedRole.Purchasing_Officer) Or IsRoleSelected(spUsrGrp(i), FixedRole.Purchasing_Manager) Then
        '                'Michelle (7/1/2011) - default to all the addresses
        '                'lsSql = "INSERT INTO USERS_ADDR VALUES('" & Common.Parse(pUser.CompanyID) & "','"
        '                'lsSql &= Common.Parse(pUser.UserID) & "','D','0','Buyer',1)"
        '                'Common.Insert2Ary(pQuery, lsSql)
        '                'lsSql = "INSERT INTO USERS_ADDR VALUES('" & Common.Parse(pUser.CompanyID) & "','"
        '                'lsSql &= Common.Parse(pUser.UserID) & "','B','0','Buyer',1)"
        '                'Common.Insert2Ary(pQuery, lsSql)
        '                'blnBuyer = False
        '                'Added by Joon on 8 Oct 2011 for issue 977 (if the user has more than 1 buyer fixed role, then only insert once)
        '                If blnInsert = False Then
        '                    lsSql = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) "
        '                    lsSql &= "SELECT '" & Common.Parse(pUser.CompanyID) & "','"
        '                    lsSql &= Common.Parse(pUser.UserID) & "','D',AM_ADDR_CODE,'Buyer',0 "
        '                    lsSql &= "FROM ADDRESS_MSTR WHERE AM_ADDR_TYPE = 'D' "
        '                    lsSql &= "AND AM_COY_ID = '" & Common.Parse(pUser.CompanyID) & "' "
        '                    Common.Insert2Ary(pQuery, lsSql)
        '                    lsSql = "INSERT INTO USERS_ADDR (UA_COY_ID, UA_USER_ID, UA_ADDR_TYPE, UA_ADDR_CODE, UA_ROLE, UA_VIEW_OPTION) "
        '                    lsSql &= "SELECT '" & Common.Parse(pUser.CompanyID) & "','"
        '                    lsSql &= Common.Parse(pUser.UserID) & "','B',AM_ADDR_CODE,'Buyer',0 "
        '                    lsSql &= "FROM ADDRESS_MSTR WHERE AM_ADDR_TYPE = 'B' "
        '                    lsSql &= "AND AM_COY_ID = '" & Common.Parse(pUser.CompanyID) & "' "
        '                    Common.Insert2Ary(pQuery, lsSql)
        '                    blnInsert = True
        '                End If
        '                blnBuyer = False

        '            ElseIf blnSK And IsRoleSelected(spUsrGrp(i), FixedRole.Store_Keeper) Then
        '                lsSql = "INSERT INTO USERS_LOCATION (UL_COY_ID, UL_ADDR_CODE, UL_USER_ID, UL_LEVEL) "
        '                lsSql &= "SELECT '" & Common.Parse(pUser.CompanyID) & "', AM_ADDR_CODE, '"
        '                lsSql &= Common.Parse(pUser.UserID) & "', 1 "
        '                lsSql &= "FROM ADDRESS_MSTR WHERE AM_ADDR_TYPE = 'D' "
        '                lsSql &= "AND AM_COY_ID = '" & Common.Parse(pUser.CompanyID) & "' "
        '                Common.Insert2Ary(pQuery, lsSql)
        '                blnSK = False
        '            ElseIf bln2nd And IsRoleSelected(spUsrGrp(i), FixedRole.Second_Level_Receiver) Then
        '                lsSql = "INSERT INTO USERS_LOCATION (UL_COY_ID, UL_ADDR_CODE, UL_USER_ID, UL_LEVEL) "
        '                lsSql &= "SELECT '" & Common.Parse(pUser.CompanyID) & "', AM_ADDR_CODE, '"
        '                lsSql &= Common.Parse(pUser.UserID) & "', 2 "
        '                lsSql &= "FROM ADDRESS_MSTR WHERE AM_ADDR_TYPE = 'D' "
        '                lsSql &= "AND AM_COY_ID = '" & Common.Parse(pUser.CompanyID) & "' "
        '                Common.Insert2Ary(pQuery, lsSql)
        '                blnSK = False
        '            End If
        '        Next
        '    End If
        'End Sub

        ''Name       : GetRoleCount
        ''Author     : kk
        ''Descption  : Return no of fixed role assigned of a company
        ''LastUpadte : 29 Nov 2004
        'Public Function GetRoleCount(ByVal pRole As String) As Integer
        '    Dim strSQL, strCnt As String

        '    strSQL = "SELECT COUNT(*) FROM USERS_USRGRP,USER_GROUP_MSTR " &
        '             "WHERE UU_USRGRP_ID=UGM_USRGRP_ID " &
        '             "AND UGM_FIXED_ROLE='" & pRole & "'  AND UU_COY_ID='" & ctx.Session("CompanyIdToken") & "' "
        '    Return objDb.GetVal(strSQL)

        'End Function

        'Name       : IsRoleSelected
        'Author     : kk
        'Descption  : check whether the Role is selected in the user group
        'LastUpadte : 22 Nov 2004
        Public Function IsRoleSelected(ByVal pUserGroup As String, ByVal pRole As FixedRole) As Boolean
            Dim strRole As String
            strRole = System.Enum.GetName(GetType(FixedRole), pRole)

            Dim strSQL As String
            pUserGroup = Replace(pUserGroup, ",", "','")
            strSQL = "SELECT * FROM USER_GROUP_MSTR WHERE UGM_USRGRP_ID IN ('" & pUserGroup & "') AND UGM_FIXED_ROLE= '" & Replace(strRole, "_", " ") & "'"
            If objDb.Exist(strSQL) = 1 Then
                Return True
            Else
                Return False
            End If
        End Function

        'Name       : UpdateUser
        'Author     : kk
        'Descption  : Update  user
        'LastUpadte : 22 Nov 2004
        Public Function UpdateUser(ByVal userID As String, ByVal companyID As String, ByVal pUser As User, ByVal pHubLevel As Boolean, ByVal pAppPackageId As String, Optional ByVal pChkUserLimit As Boolean = True, Optional ByVal pChkEmailChange As Boolean = False) As Boolean
            UpdateUser = False
            Dim Query(0) As String
            Dim strSql1 As String

            Try
                'kk.remark.18/01/2004 pChkUserLimit=False mean not need chk for user limit. eg- when user update their own details
                'but in update user activation needed to chk
                If pChkUserLimit And pUser.Status = "A" Then
                    If pHubLevel Then
                        If IsExceedAdminLimit(pUser.CompanyID, pUser.UserID) Then
                            Return False
                        End If
                    Else
                        If IsExceedUserLimit(pUser.CompanyID) Then
                            Return False
                        End If
                    End If
                End If

                'This os to cater if the user update status, if chg from I -> A will required to chk
                'user limit
                Dim strSQL, strStatus As String
                strSQL = "SELECT UM_STATUS FROM USER_MSTR WHERE UM_USER_ID='" & Common.Parse(pUser.UserID) &
                           "' AND UM_COY_ID='" & Common.Parse(pUser.CompanyID) & "' AND UM_DELETED<>'Y'"
                strStatus = objDb.GetVal(strSQL)
                If UCase(strStatus) = "I" And pUser.Status = "A" Then
                    If pHubLevel Then
                        If IsExceedAdminLimit(pUser.CompanyID, pUser.UserID) Then
                            Return False
                        End If
                    Else
                        If IsExceedUserLimit(pUser.CompanyID) Then
                            Return False
                        End If
                    End If
                End If

                If IsExceedReportUser(pUser.CompanyID, pUser) And pUser.Status = "A" Then Return False

                If pHubLevel Then
                    lsSql = "UPDATE USER_MSTR SET UM_USER_NAME='" & Common.Parse(pUser.Name) & "'," &
                            "UM_EMAIL='" & Common.Parse(pUser.Email) & "'," &
                            "UM_STATUS='" & Common.Parse(pUser.Status) & "'," &
                            "UM_MOD_BY='" & userID & "'," &
                            "UM_MOD_DT=GETDATE()," &
                            "UM_DELETED='" & Common.Parse(pUser.DeleteInd) & "', " &
                            "UM_MASS_APP='" & Common.Parse(pUser.MassApp) & "' " &
                           "WHERE UM_USER_ID='" & pUser.UserID & "' AND UM_COY_ID='" & companyID & "'" ''ctx.Session("CompanyIdToken")
                    Common.Insert2Ary(Query, lsSql)
                Else
                    lsSql = "UPDATE USER_MSTR SET UM_USER_NAME='" & Common.Parse(pUser.Name) & "'," &
                            "UM_DEPT_ID='" & Common.Parse(pUser.DeptID) & "'," &
                            "UM_EMAIL='" & Common.Parse(pUser.Email) & "'," &
                            "UM_TEL_NO='" & Common.Parse(pUser.PhoneNo) & "'," &
                            "UM_FAX_NO='" & Common.Parse(pUser.FaxNo) & "'," &
                            "UM_DESIGNATION='" & Common.Parse(pUser.Designation) & "'," &
                            "UM_APP_LIMIT=" & Common.ConvertMoney(pUser.ApproveLimit) & "," &
                            "UM_PO_APP_LIMIT=" & Common.ConvertMoney(pUser.POApproveLimit) & "," &
                            "UM_INVOICE_APP_LIMIT=" & Common.ConvertMoney(pUser.InvoiceApproveLimit) & "," &
                            "UM_STATUS='" & Common.Parse(pUser.Status) & "'," &
                            "UM_MOD_BY='" & userID & "'," &
                            "UM_MOD_DT=GETDATE()," &
                            "UM_RECORD_COUNT = " & pUser.PageCount & ", " &
                            "UM_EMAIL_CC = '" & Common.Parse(pUser.CCList) & "', " &
                            "UM_STAFF_CLAIM_EMAIL = '" & pUser.SCEmailOnOff & "', " &
                            "UM_DELETED='" & Common.Parse(pUser.DeleteInd) & "', "

                    If (pUser.MassApp) = Nothing Then
                        lsSql &= "UM_MASS_APP=UM_MASS_APP, "
                    Else
                        lsSql &= "UM_MASS_APP='" & Common.Parse(pUser.MassApp) & "', "
                    End If

                    If (pUser.MrsMassApp) = Nothing Then
                        lsSql &= "UM_MRS_MASS_APP=UM_MRS_MASS_APP, "
                    Else
                        lsSql &= "UM_MRS_MASS_APP='" & Common.Parse(pUser.MrsMassApp) & "', "
                    End If

                    If (pUser.StockTypeSpot) = Nothing Then
                        lsSql &= "UM_STK_TYPE_SPOT=UM_STK_TYPE_SPOT, "
                    Else
                        lsSql &= "UM_STK_TYPE_SPOT='" & Common.Parse(pUser.StockTypeSpot) & "', "
                    End If

                    If (pUser.StockTypeStock) = Nothing Then
                        lsSql &= "UM_STK_TYPE_STOCK=UM_STK_TYPE_STOCK, "
                    Else
                        lsSql &= "UM_STK_TYPE_STOCK='" & Common.Parse(pUser.StockTypeStock) & "', "
                    End If

                    If (pUser.StockTypeMro) = Nothing Then
                        lsSql &= "UM_STK_TYPE_MRO=UM_STK_TYPE_MRO "
                    Else
                        lsSql &= "UM_STK_TYPE_MRO='" & Common.Parse(pUser.StockTypeMro) & "' "
                    End If

                    lsSql &= "WHERE UM_USER_ID='" & pUser.UserID & "' AND UM_COY_ID='" & companyID & "' "

                    Common.Insert2Ary(Query, lsSql)

                    If pChkEmailChange = True Then
                        lsSql = "UPDATE USER_MSTR SET UM_POLICY_AGREE_DATE=NULL " &
                                                    "WHERE UM_USER_ID='" & pUser.UserID & "' AND UM_COY_ID='" & companyID & "'"
                        Common.Insert2Ary(Query, lsSql)
                    End If
                End If

                'Added by Joon on 22 June 2011
                'If Fixed role = Purchasing Officer
                If pUser.strFixedRole = Replace(FixedRole.Purchasing_Officer.ToString, "_", " ") Then
                    Dim dt1 As New DataTable
                    Dim i As Integer

                    'strSql1 = "SELECT CT_ID,CT_NAME,CT_PARENT_ID FROM commodity_type WHERE CT_LAST_LVL=1"
                    strSql1 = "SELECT CT_ROOT_PREFIX FROM commodity_type WHERE CT_PARENT_ID IS NULL"
                    dt1 = objDb.FillDt(strSql1)
                    For i = 0 To dt1.Rows.Count - 1
                        strSql1 = "SELECT '*' " _
                             & "FROM user_assign " _
                             & "WHERE UA_COY_ID='" & Common.Parse(pUser.CompanyID) & "' " _
                             & "AND UA_USER_ID='" & Common.Parse(pUser.UserID) & "' " _
                             & "AND UA_TYPE='CT' AND UA_FIXED_ROLE='" & Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "' " _
                             & "AND UA_ASSIGN_VALUE=" & dt1.Rows(i).Item("CT_ROOT_PREFIX")
                        If objDb.Exist(strSql1) = 0 Then
                            lsSql = "INSERT INTO user_assign(" &
                                   "UA_COY_ID,UA_USER_ID,UA_FIXED_ROLE,UA_TYPE,UA_ASSIGN_VALUE) " &
                                   "VALUES('" &
                                   Common.Parse(pUser.CompanyID) & "','" & Common.Parse(pUser.UserID) & "', '" &
                                   Common.Parse(Replace(FixedRole.Purchasing_Officer.ToString, "_", " ")) & "','CT'," &
                                   dt1.Rows(i).Item("CT_ROOT_PREFIX") & ")"
                            Common.Insert2Ary(Query, lsSql)
                        End If
                    Next
                End If

                UpdateUsers_UsrGrp(userID, Query, pUser, pAppPackageId)

                If objDb.BatchExecute(Query) Then
                    strUserMassage = Common.RecordSave
                    Return True
                End If

            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        ''Name       : UpdateMassApproval
        ''Author     : kk
        ''Descption  : Update user mass apporval right for OA aft added new
        ''LastUpadte : 29 Nov 2004
        'Public Function UpdateMassApproval(ByVal pUserId As String, ByVal pValue As Boolean) As Boolean
        '    Dim strSQL As String

        '    If pValue Then
        '        strSQL = "Update USER_MSTR SET UM_MASS_APP='Y' WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    Else
        '        strSQL = "Update USER_MSTR SET UM_MASS_APP='N' WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    End If

        '    If objDb.Execute(strSQL) Then
        '        strUserMassage = Common.RecordSave
        '        Return True
        '    Else
        '        strUserMassage = Common.RecordNotSave
        '        Return False
        '    End If
        'End Function

        'Public Function UpdateInvoiceMassApproval(ByVal pUserId As String, ByVal pValue As Boolean) As Boolean
        '    Dim strSQL As String

        '    If pValue Then
        '        strSQL = "Update USER_MSTR SET UM_INVOICE_MASS_APP='Y' WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    Else
        '        strSQL = "Update USER_MSTR SET UM_INVOICE_MASS_APP='N' WHERE UM_USER_ID='" & pUserId & "' AND UM_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    End If

        '    If objDb.Execute(strSQL) Then
        '        strUserMassage = Common.RecordSave
        '        Return True
        '    Else
        '        strUserMassage = Common.RecordNotSave
        '        Return False
        '    End If
        'End Function

        ''Name       : DeleteUsersRole
        ''Author     : kk
        ''Descption  : Delete User Role for a particular user
        ''LastUpadte : 22 Nov 2004
        'Private Sub DeleteUsersRole(ByVal pUserId As String)
        '    lsSql = "DELETE FROM USERS_USRGRP WHERE UU_USER_ID='" & pUserId & "' AND UU_COY_ID='" & ctx.Session("CompanyId") & "'"
        '    objDb.Execute(lsSql)
        'End Sub

        'Public Function DelUser(ByVal pUserId As String, ByVal pHubLevel As Boolean) As Boolean
        '    Dim strCompId As String
        '    Dim Query(0) As String

        '    If pHubLevel Then
        '        strCompId = ctx.Session("CompanyIdToken")
        '    Else
        '        strCompId = ctx.Session("CompanyId")
        '    End If

        '    Try
        '        ' User are not allowed to delete their own account
        '        If pUserId = ctx.Session("UserID") Then
        '            strUserMassage = "Deletion is not allowed. User are not allowed to delete own account"
        '            Return False
        '        End If

        '        If Not pHubLevel Then
        '            ' Normal user
        '            If Not IsNormalAdmin() And Not IsAdmin() Then
        '                strUserMassage = "Deletion is not allowed. User role is not appropriate"
        '                Return False
        '            End If

        '            ' Normal admin
        '            If IsNormalAdmin() Then
        '                If IsAdmin(pUserId, strCompId) Or IsNormalAdmin(pUserId, strCompId) Then
        '                    strUserMassage = "Deletion is not allowed. User role is not appropriate"
        '                    Return False
        '                End If
        '            End If
        '        End If

        '        If IsTieToTxn(pUserId, strCompId) Then
        '            strUserMassage = Common.RecordTiedTxn
        '            Return False
        '        ElseIf IsApprovalGrp(pUserId, strCompId) Then
        '            strUserMassage = Common.RecordTiedTxn
        '            Return False
        '        Else

        '            lsSql = "UPDATE USER_MSTR SET UM_DELETED='Y',UM_MOD_BY='" &
        '            Common.Parse(ctx.Session("UserID")) & "',UM_MOD_DT=" & Common.ConvertDate(Now()) & " WHERE UM_USER_ID='" &
        '            Common.Parse(pUserId) & "' AND UM_COY_ID='" &
        '            Common.Parse(strCompId) & "'"
        '            Common.Insert2Ary(Query, lsSql)

        '            'Michelle (20/9/2010) - Add in the 'From' in the Delete statement
        '            'lsSql = "DELETE USERS_USRGRP WHERE UU_USER_ID='" & Common.Parse(pUserId) & "'" & _
        '            lsSql = "DELETE FROM USERS_USRGRP WHERE UU_USER_ID='" & Common.Parse(pUserId) & "'" &
        '            " AND UU_COY_ID='" &
        '            Common.Parse(strCompId) & "'"
        '            Common.Insert2Ary(Query, lsSql)

        '            If objDb.BatchExecute(Query) Then
        '                'Michelle (7/1/2011) - To delete from user_addr
        '                Dim objDb1 As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))

        '                lsSql = "DELETE FROM USERS_ADDR WHERE UA_USER_ID='" & Common.Parse(pUserId) & "'" &
        '                " AND UA_COY_ID='" &
        '                Common.Parse(strCompId) & "'"
        '                If objDb1.Execute(lsSql) Then
        '                    strUserMassage = Common.RecordDelete
        '                    Return True
        '                Else
        '                    strUserMassage = Common.RecordNotDelete
        '                    Return False
        '                End If
        '            End If
        '        End If
        '    Catch errExp As CustomException
        '        Common.TrwExp(errExp)
        '    Catch errExp1 As Exception
        '        Common.TrwExp(errExp1)
        '    End Try
        'End Function

        ''Name       : IsTieToTxn
        ''Author     : kk
        ''Descption  : Check for particular user is tie to any txn
        ''LastUpadte : 03 Dec 2004
        ''remark     : need to change connection string to access different database
        'Private Function IsTieToTxn(ByVal pUserId As String, ByVal pCompId As String) As Boolean
        '    Dim i As Integer
        '    Dim strSQL, Query(0), Query2(0) As String

        '    ' access eProcure (Wheel) database
        '    If IsValidAppPackage(pCompId, EnumAppPackage.eProcure) Then

        '        Dim objDbProcure As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))

        '        'PR - PRM_BUYER_ID - PRM_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM PR_MSTR WHERE PRM_BUYER_ID ='" & Common.Parse(pUserId) & "' AND PRM_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'PO - POM_BUYER_ID - POM_B_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM PO_MSTR WHERE POM_BUYER_ID ='" & Common.Parse(pUserId) & "' AND POM_B_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'DO - DOM_CREATED_BY - DOM_S_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM DO_MSTR WHERE DOM_CREATED_BY ='" & Common.Parse(pUserId) & "' AND DOM_S_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'GRN - GM_CREATED_BY,GM_LEVEL2_USER - GM_B_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM GRN_MSTR WHERE (GM_CREATED_BY ='" & Common.Parse(pUserId) & "' OR GM_LEVEL2_USER='" & Common.Parse(pUserId) & "') AND GM_B_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'Invoice - IM_CREATED_BY - IM_B_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM INVOICE_MSTR WHERE IM_CREATED_BY ='" & Common.Parse(pUserId) & "' AND IM_B_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'PO Cancelation - PCM_REQ_BY - PCM_B_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM PO_CR_MSTR WHERE PCM_REQ_BY ='" & Common.Parse(pUserId) & "' AND PCM_B_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'PR Approval - PRA_ACTIVE_AO - PRM_COY_ID
        '        'strSQL = "SELECT COUNT(*) FROM PR_APPROVAL WHERE PRA_ACTIVE_AO ='" & Common.Parse(pUserId) & "'"
        '        strSQL = "SELECT COUNT(*) FROM PR_APPROVAL, PR_MSTR WHERE PRM_PR_INDEX = PRA_PR_INDEX AND PRA_ACTIVE_AO ='" & Common.Parse(pUserId) & "' AND PRM_COY_ID = '" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'RFQ - RM_Created_By - RM_Coy_ID
        '        strSQL = "SELECT COUNT(*) FROM RFQ_MSTR WHERE RM_Created_By ='" & Common.Parse(pUserId) & "' AND RM_Coy_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'RFQ Replies - RRM_Created_By - RRM_V_Company_ID
        '        strSQL = "SELECT COUNT(*) FROM RFQ_REPLIES_MSTR WHERE RRM_Created_By ='" & Common.Parse(pUserId) & "' AND RRM_V_Company_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        'Temp Product Request - PM_ENT_BY - PM_S_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM PRODUCT_MSTR_TEMP WHERE PM_STATUS = 1 AND PM_ENT_BY = '" & Common.Parse(pUserId) & "' AND PM_S_COY_ID = '" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query, strSQL)

        '        For i = 0 To Query.Length - 1
        '            If objDbProcure.GetVal(Query(i)) Then
        '                objDbProcure = Nothing
        '                Return True
        '            End If
        '        Next

        '        objDbProcure = Nothing
        '    End If

        '    ' for eRFP
        '    If IsValidAppPackage(pCompId, EnumAppPackage.eRFP) Then

        '        Dim objDbRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        '        'RFP - RM_Created_By - RM_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM RFP_MSTR  WHERE RM_CREATED_BY ='" & Common.Parse(pUserId) & "' AND RM_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query2, strSQL)

        '        'ADDENDUM, CORRIGENDUM, CLARIFICATION - AM_CREATED_BY, AM_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM ADDENDUM_MSTR  WHERE AM_CREATED_BY ='" & Common.Parse(pUserId) & "' AND AM_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query2, strSQL)

        '        'CREATE QUERY  - RQM_CREATED_BY, RQM_V_COY_ID
        '        strSQL = "SELECT COUNT(*) FROM RFP_QUERY_MSTR  WHERE RQM_CREATED_BY ='" & Common.Parse(pUserId) & "' AND RQM_V_COY_ID ='" & Common.Parse(pCompId) & "'"
        '        Common.Insert2Ary(Query2, strSQL)

        '        For i = 0 To Query2.Length - 1
        '            If objDbRFP.GetVal(Query2(i)) Then
        '                objDbRFP = Nothing
        '                Return True
        '            End If
        '        Next

        '        objDbRFP = Nothing
        '    End If

        '    Return False
        'End Function

        ''Name       : IsApprovalGrp
        ''Author     : kk
        ''Descption  : Check for particular user is tie to any approval group
        ''LastUpadte : 03 Dec 2004
        ''remark     : need to change connection string to access different database
        'Private Function IsApprovalGrp(ByVal pUserId As String, ByVal pCompId As String) As Boolean
        '    Dim i As Integer
        '    Dim strSQL, Query(0) As String
        '    'Dim objDbcom As New EAD.DBCom

        '    ' for eProcure
        '    If IsValidAppPackage(pCompId, EnumAppPackage.eProcure) Then
        '        Dim objDbProcure As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        '        'APPROVAL GRP - AGA_AO,AGA_A_AO - 
        '        strSQL = "SELECT COUNT(*) FROM APPROVAL_GRP_AO WHERE (AGA_AO ='" & Common.Parse(pUserId) & "' OR AGA_A_AO = '" & Common.Parse(pUserId) & "')"

        '        If objDbProcure.GetVal(strSQL) Then
        '            objDbProcure = Nothing
        '            Return True
        '        End If

        '        objDbProcure = Nothing
        '    End If

        '    ' for eRFP
        '    If IsValidAppPackage(pCompId, EnumAppPackage.eRFP) Then
        '        Dim objDbRFP As New EAD.DBCom(ConfigurationSettings.AppSettings("eRFPPath"))

        '        strSQL = "SELECT COUNT(*) FROM RFP_OFFICER_GRP_DETAIL WHERE ((ROGD_USER_ID ='" & Common.Parse(pUserId) & "' OR ROGD_USER_ALT_ID = '" & Common.Parse(pUserId) & "') "
        '        strSQL &= "AND ROGD_COY_ID = '" & Common.Parse(pCompId) & "')"
        '        Common.Insert2Ary(Query, strSQL)

        '        strSQL = "SELECT COUNT(*) FROM RFP_DOCOWNER WHERE (RDO_USER_ID ='" & Common.Parse(pUserId) & "' AND RDO_COY_ID = '" & Common.Parse(pCompId) & "')"
        '        Common.Insert2Ary(Query, strSQL)

        '        For i = 0 To Query.Length - 1
        '            If objDbRFP.GetVal(Query(i)) Then
        '                objDbRFP = Nothing
        '                Return True
        '            End If
        '        Next

        '        objDbRFP = Nothing
        '    End If

        '    Return False
        'End Function

        'Public Function IsTieToAnyUserGrp(ByVal strCoyId As String, ByVal strUserId As String, ByVal strApp As String) As Boolean
        '    Dim strsql As String
        '    Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("SSO"))
        '    strsql = "SELECT '*' FROM USERS_USRGRP WHERE UU_COY_ID = '" & Common.Parse(strCoyId) & "' "
        '    strsql &= "AND UU_USER_ID = '" & Common.Parse(strUserId) & "' "
        '    strsql &= "AND UU_APP_PKG <> '" & strApp & "'"
        '    If objDb.Exist(strsql) > 0 Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        '    objDb = Nothing
        'End Function

        'Public Function IsValidAppPackage(ByVal pCompanyId As String, ByVal pPackageId As EnumAppPackage) As Boolean
        '    Dim strSQL As String

        '    strSQL = "SELECT 1 FROM COMPANY_APPLICATION " &
        '        " WHERE (CA_COY_ID = '" & pCompanyId & "') AND (CA_APP_ID = '" & pPackageId.ToString & "')"

        '    Return IIf(objDb.Exist(strSQL) = 1, True, False)
        'End Function

        ''Name       : SearchAddress
        ''Author     : kk
        ''Descption  : Retrieve delevery address of a company 'TYPE=D'
        ''LastUpadte : 29 Nov 2004
        ''Public Function SearchUserAddress(ByVal pUserId As String, Optional ByVal iLevel As Integer = 0, Optional ByVal blnBuyer As Boolean = False, Optional ByVal pFixedRole As FixedRole = FixedRole.Buyer) As DataSet
        'Public Function SearchUserAddress(ByVal pUserId As String, ByVal strCode As String, ByVal strCity As String, ByVal strState As String, Optional ByVal iLevel As Integer = 0, Optional ByVal blnBuyer As Boolean = False, Optional ByVal pFixedRole As FixedRole = FixedRole.Buyer, Optional ByVal strCountry As String = "") As DataSet
        '    Dim strSQL As String
        '    Dim strTemp As String

        '    'if iLevel pass in null mean the user is storekeper and 2th level receiver
        '    'In this case the user will share the delivery addr for both user group.
        '    'but regarding mic a user shd not have this 2 user group at e same time.

        '    ' modified by Ai chu
        '    Select Case blnBuyer
        '        Case True
        '            ' iLevel : 0 - Delivery Address
        '            '          1 - Billing Address
        '            strSQL = "SELECT '*' FROM USERS_ADDR WHERE UA_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '            strSQL &= "AND UA_USER_ID = '" & pUserId & "' "
        '            strSQL &= "AND UA_ADDR_TYPE = '" & IIf(iLevel = 0, "D", "B") & "' "
        '            strSQL &= "AND UA_VIEW_OPTION = 1"
        '            'Michelle (18/10/2010) - To cater for MYSQL
        '            strSQL &= "; "
        '            If objDb.Exist(strSQL) > 0 Then
        '                ' authorized all address
        '                strSQL = "SELECT 'Y' AS AM_SELECTED, AM_ADDR_CODE, (" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "AM_ADDR_LINE2", "AM_ADDR_LINE3") & ") AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_POSTCODE, AM_STATE, AM_CITY, AM_COUNTRY, "
        '                strSQL &= "ISNULL(a.CODE_DESC,'n.a.') AS STATE, b.CODE_DESC AS COUNTRY "
        '                strSQL &= "FROM ADDRESS_MSTR "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS A ON AM_STATE = A.CODE_ABBR AND A.CODE_CATEGORY = 'S' "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS B ON AM_COUNTRY = B.CODE_ABBR AND B.CODE_CATEGORY = 'CT' "
        '                strSQL &= "WHERE AM_ADDR_TYPE = '" & IIf(iLevel = 0, "D", "B") & "' "
        '                strSQL &= "AND AM_COY_ID = '" & ctx.Session("CompanyId") & "' "

        '                If strCode <> "" Then
        '                    strTemp = Common.BuildWildCard(strCode)
        '                    strSQL &= "AND AM_ADDR_CODE " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strCity <> "" Then
        '                    strTemp = Common.BuildWildCard(strCity)
        '                    strSQL &= "AND AM_CITY " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strState <> "" Then
        '                    strSQL &= "AND AM_STATE = '" & strState & "' "
        '                End If

        '                If strCountry <> "" Then
        '                    strSQL &= "AND AM_COUNTRY = '" & strCountry & "' "
        '                End If
        '                'Michelle (20/9/2010) - to remove the following line as it causes error in MYSQL
        '                'strSQL &= "SELECT 0"
        '                strSQL &= "; SELECT 0"
        '            Else
        '                'strSQL = " select b.*,c.CODE_DESC as COUNTRY from (" & _
        '                '            " SELECT a.*,CODE_DESC as STATE  from ( " & _
        '                '            " SELECT (CASE UA_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, AM_ADDR_CODE,(AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1,AM_POSTCODE, AM_STATE,AM_CITY,AM_COUNTRY " & _
        '                '            " FROM ADDRESS_MSTR LEFT JOIN USERS_ADDR ON AM_ADDR_CODE=UA_ADDR_CODE AND AM_COY_ID=UA_COY_ID AND UA_USER_ID='" & pUserId & "' AND UA_ADDR_TYPE = AM_ADDR_TYPE " & _
        '                '            " WHERE AM_ADDR_TYPE='" & IIf(iLevel = 0, "D", "B") & "' AND AM_COY_ID='" & ctx.Session("CompanyId") & "')a,Code_MSTR " & _
        '                '            " WHERE AM_STATE=CODE_ABBR AND CODE_CATEGORY='S')b,CODE_MSTR c " & _
        '                '            " WHERE AM_COUNTRY=c.CODE_ABBR AND c.CODE_CATEGORY='CT'; "
        '                strSQL = "SELECT (CASE UA_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, "
        '                'Michelle (27/9/2010) - To cater for MYSQL syntax
        '                'strSQL &= "AM_ADDR_CODE, (AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_ADDR_CODE, (" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "AM_ADDR_LINE2", "AM_ADDR_LINE3") & ") AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_POSTCODE, AM_STATE, AM_CITY, AM_COUNTRY, ISNULL(B.CODE_DESC,'n.a.') AS STATE, C.CODE_DESC AS COUNTRY "
        '                strSQL &= "FROM ADDRESS_MSTR "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "
        '                strSQL &= "LEFT JOIN USERS_ADDR ON AM_ADDR_CODE = UA_ADDR_CODE AND AM_COY_ID = UA_COY_ID "
        '                strSQL &= "AND UA_USER_ID = '" & pUserId & "' AND UA_ADDR_TYPE = AM_ADDR_TYPE "
        '                strSQL &= "WHERE AM_ADDR_TYPE='" & IIf(iLevel = 0, "D", "B") & "' "
        '                strSQL &= "AND AM_COY_ID='" & ctx.Session("CompanyId") & "' "

        '                If strCode <> "" Then
        '                    strTemp = Common.BuildWildCard(strCode)
        '                    strSQL &= "AND AM_ADDR_CODE " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strCity <> "" Then
        '                    strTemp = Common.BuildWildCard(strCity)
        '                    strSQL &= "AND AM_CITY " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strState <> "" Then
        '                    strSQL &= "AND AM_STATE = '" & strState & "' "
        '                End If

        '                If strCountry <> "" Then
        '                    strSQL &= "AND AM_COUNTRY = '" & strCountry & "' "
        '                End If
        '                ' ai chu modified on 09/11/2005
        '                ' if return > 0 records, means not all addresses are selected
        '                strSQL &= "; select COUNT(*) from address_mstr a where am_addr_type='D' "
        '                strSQL &= "and am_coy_id='" & ctx.Session("CompanyId") & "' "
        '                strSQL &= "and not exists (select * from users_addr b "
        '                strSQL &= "where a.am_addr_code=b.ua_addr_code and am_coy_id=ua_coy_id "
        '                strSQL &= "and ua_user_id='" & pUserId & "' "
        '                strSQL &= "and ua_coy_id='" & ctx.Session("CompanyId") & "') "
        '                'strSQL &= "SELECT (SELECT COUNT(UA_ADDR_CODE) FROM USERS_ADDR "
        '                'strSQL &= "WHERE UA_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '                'strSQL &= "AND UA_USER_ID = '" & pUserId & "' AND UA_ADDR_TYPE = 'D') AS Addr1, "
        '                'strSQL &= "(SELECT COUNT(AM_ADDR_CODE) FROM ADDRESS_MSTR "
        '                'strSQL &= "WHERE AM_COY_ID = '" & ctx.Session("CompanyId") & "' AND AM_ADDR_TYPE = 'D') AS Addr2"
        '            End If

        '        Case False
        '            If iLevel = 0 Then
        '                'strSQL = " select b.*,c.CODE_DESC as COUNTRY from (" & _
        '                '         " SELECT a.*,CODE_DESC as STATE  from ( " & _
        '                '         " SELECT (CASE UL_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, AM_ADDR_CODE,(AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1,AM_POSTCODE, AM_STATE,AM_CITY,AM_COUNTRY " & _
        '                '         " FROM ADDRESS_MSTR LEFT JOIN USERS_LOCATION ON AM_ADDR_CODE=UL_ADDR_CODE AND AM_COY_ID=UL_COY_ID AND UL_USER_ID='" & pUserId & "'" & _
        '                '         " WHERE AM_ADDR_TYPE='D' AND AM_COY_ID='" & ctx.Session("CompanyId") & "')a,Code_MSTR " & _
        '                '         " WHERE AM_STATE=CODE_ABBR AND CODE_CATEGORY='S')b,CODE_MSTR c " & _
        '                '         " WHERE AM_COUNTRY=c.CODE_ABBR AND c.CODE_CATEGORY='CT'; "
        '                strSQL = "SELECT (CASE UL_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, "
        '                'strSQL &= "AM_ADDR_CODE, (AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1, "
        '                'Michelle (27/9/2010) - To cater for MYSQL syntax
        '                'strSQL &= "AM_ADDR_CODE, (AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_ADDR_CODE, (" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "AM_ADDR_LINE2", "AM_ADDR_LINE3") & ") AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_POSTCODE, AM_STATE, AM_CITY, AM_COUNTRY, ISNULL(B.CODE_DESC,'n.a.') AS STATE, C.CODE_DESC AS COUNTRY "
        '                strSQL &= "FROM ADDRESS_MSTR "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "
        '                strSQL &= "LEFT JOIN USERS_LOCATION ON AM_ADDR_CODE = UL_ADDR_CODE AND AM_COY_ID = UL_COY_ID AND UL_USER_ID = '" & pUserId & "' "
        '                strSQL &= "WHERE AM_ADDR_TYPE = 'D' AND AM_COY_ID = '" & ctx.Session("CompanyId") & "' "

        '                If strCode <> "" Then
        '                    strTemp = Common.BuildWildCard(strCode)
        '                    strSQL &= "AND AM_ADDR_CODE " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strCity <> "" Then
        '                    strTemp = Common.BuildWildCard(strCity)
        '                    strSQL &= "AND AM_CITY " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strState <> "" Then
        '                    strSQL &= "AND AM_STATE = '" & strState & "' "
        '                End If

        '                If strCountry <> "" Then
        '                    strSQL &= "AND AM_COUNTRY = '" & strCountry & "' "
        '                End If
        '            Else
        '                'strSQL = " select b.*,c.CODE_DESC as COUNTRY from (" & _
        '                '        " SELECT a.*,CODE_DESC as STATE  from ( " & _
        '                '        " SELECT (CASE UL_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, AM_ADDR_CODE,(AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1,AM_POSTCODE, AM_STATE,AM_CITY,AM_COUNTRY " & _
        '                '        " FROM ADDRESS_MSTR LEFT JOIN USERS_LOCATION ON AM_ADDR_CODE=UL_ADDR_CODE AND AM_COY_ID=UL_COY_ID  AND  UL_USER_ID='" & pUserId & "' AND UL_LEVEL=" & iLevel & _
        '                '        " WHERE AM_ADDR_TYPE='D' AND AM_COY_ID='" & ctx.Session("CompanyId") & "')a,Code_MSTR " & _
        '                '        " WHERE AM_STATE=CODE_ABBR AND CODE_CATEGORY='S')b,CODE_MSTR c " & _
        '                '        " WHERE AM_COUNTRY=c.CODE_ABBR AND c.CODE_CATEGORY='CT'; "
        '                strSQL = "SELECT (CASE UL_USER_ID WHEN  '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS AM_SELECTED, "
        '                'strSQL &= "AM_ADDR_CODE, (AM_ADDR_LINE1 + ' ' + AM_ADDR_LINE2 + ' ' + AM_ADDR_LINE3) AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_ADDR_CODE, (" & objDb.Concat(" ", "", "AM_ADDR_LINE1", "AM_ADDR_LINE2", "AM_ADDR_LINE3") & ") AS AM_ADDR_LINE1, "
        '                strSQL &= "AM_POSTCODE, AM_STATE, AM_CITY, AM_COUNTRY, ISNULL(B.CODE_DESC,'n.a.') AS STATE, C.CODE_DESC AS COUNTRY "
        '                strSQL &= "FROM ADDRESS_MSTR "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS B ON AM_STATE = B.CODE_ABBR AND B.CODE_CATEGORY = 'S' "
        '                strSQL &= "LEFT JOIN CODE_MSTR AS C ON AM_COUNTRY = C.CODE_ABBR AND C.CODE_CATEGORY = 'CT' "
        '                strSQL &= "LEFT JOIN USERS_LOCATION ON AM_ADDR_CODE = UL_ADDR_CODE AND AM_COY_ID = UL_COY_ID "
        '                strSQL &= "AND UL_USER_ID = '" & pUserId & "' "
        '                strSQL &= "AND UL_LEVEL = " & iLevel & " "
        '                strSQL &= "WHERE AM_ADDR_TYPE = 'D' AND AM_COY_ID = '" & ctx.Session("CompanyId") & "' "

        '                If strCode <> "" Then
        '                    strTemp = Common.BuildWildCard(strCode)
        '                    strSQL &= "AND AM_ADDR_CODE " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strCity <> "" Then
        '                    strTemp = Common.BuildWildCard(strCity)
        '                    strSQL &= "AND AM_CITY " & Common.ParseSQL(strTemp) & " "
        '                End If

        '                If strState <> "" Then
        '                    strSQL &= "AND AM_STATE = '" & strState & "' "
        '                End If

        '                If strCountry <> "" Then
        '                    strSQL &= "AND AM_COUNTRY = '" & strCountry & "' "
        '                End If
        '            End If
        '            ' ai chu modified on 09/11/2005
        '            ' if return > 0 records, means not all addresses are selected
        '            'Michelle (18/10/2010) - To cater for MYSQL
        '            strSQL &= "; "
        '            strSQL &= "select COUNT(*) from address_mstr a where am_addr_type='D' "
        '            strSQL &= "and am_coy_id='" & ctx.Session("CompanyId") & "' "
        '            strSQL &= "and not exists (select * from users_location b "
        '            strSQL &= "where a.am_addr_code=b.ul_addr_code and am_coy_id=ul_coy_id "
        '            strSQL &= "and ul_user_id='" & pUserId & "' "
        '            strSQL &= "and ul_coy_id='" & ctx.Session("CompanyId") & "') "
        '            'strSQL &= "SELECT (SELECT COUNT(UL_ADDR_CODE) FROM USERS_LOCATION "
        '            'strSQL &= "WHERE UL_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '            'strSQL &= "AND UL_USER_ID = '" & pUserId & "' AND UL_LEVEL = " & iLevel & " ) AS Addr1, "
        '            'strSQL &= "(SELECT COUNT(AM_ADDR_CODE) FROM ADDRESS_MSTR WHERE AM_COY_ID = '" & ctx.Session("CompanyId") & "' "
        '            'strSQL &= "AND AM_ADDR_TYPE = 'D') AS Addr2"
        '            'UL_ADDR_CODE
        '    End Select
        '    Return objDb.FillDs(strSQL)
        'End Function

        ''Name       : SearchAddress
        ''Author     : kk
        ''Descption  : Retrieve delevery address of a company 'TYPE=D'
        ''LastUpadte : 29 Nov 2004
        'Public Function SearchUserDept(ByVal pUserId As String, Optional ByVal view As Integer = 0) As DataSet
        '    Dim strSQL As String
        '    'Zulham 19072018 - PAMB
        '    If view = 0 Then
        '        strSQL = "SELECT DISTINCT (CASE FUD_USER_ID WHEN '" & pUserId & "' THEN 'Y' ELSE 'N' END) AS CDM_SELECTED,CDM_DEPT_CODE,CDM_DEPT_NAME " &
        '                             "FROM COMPANY_DEPT_MSTR LEFT JOIN FINANCE_USER_DEPARTMENT  " &
        '                            "ON CDM_DEPT_CODE=FUD_DEPT_CODE AND FUD_USER_ID='" & pUserId & "' " &
        '                             "AND FUD_COY_ID = CDM_COY_ID " &
        '                             "WHERE   CDM_COY_ID='" & ctx.Session("CompanyId") & "' AND CDM_DELETED='N'"
        '    Else
        '        strSQL = "SELECT DISTINCT (CASE IFNULL(FUD_VIEWOPTION,2) WHEN 2 THEN 'Y' ELSE 'N' END) AS CDM_SELECTED,CDM_DEPT_CODE,CDM_DEPT_NAME " &
        '                 "FROM COMPANY_DEPT_MSTR LEFT JOIN FINANCE_USER_DEPARTMENT  " &
        '                 "ON CDM_DEPT_CODE=FUD_DEPT_CODE AND FUD_USER_ID='" & pUserId & "' " &
        '                 "AND FUD_COY_ID = CDM_COY_ID " &
        '                 "WHERE   CDM_COY_ID='" & ctx.Session("CompanyId") & "' AND CDM_DELETED='N'"
        '    End If

        '    Return objDb.FillDs(strSQL)
        'End Function

        'Name       : UpdatePasswordHistory
        'Author     : kk
        'Descption  : Update user password history
        'LastUpadte : 11 Dec 2004
        Private Function UpdatePasswordHistory(ByVal pUserId As String, ByVal pCompId As String, ByVal pPwd As String) As Boolean
            Dim strSQL As String
            strSQL = "INSERT INTO USER_PASSWORD_HISTORY " &
                     "VALUES('" & Common.Parse(pUserId) & "','" & Common.Parse(pCompId) & "', CAST('" & pPwd & "' AS BINARY)," & Common.ConvertDate(Now) & ")"
            If objDb.Execute(strSQL) Then
                Return True
            Else
                Return False
            End If
        End Function

        'Public Function IsUserViewAllDept(ByVal pUser As String) As Integer
        '    Dim strSQL As String

        '    strSQL = "SELECT * FROM FINANCE_USER_DEPARTMENT WHERE FUD_COY_ID='" & ctx.Session("CompanyId") & "' AND FUD_USER_ID='" & pUser & "' AND FUD_VIEWOPTION='1'"
        '    Return objDb.Exist(strSQL)
        'End Function

        'Friend Function SSO() As String
        '    lsSql = "SELECT UM_LOGOUT FROM USERS_MSTR WHERE UM_USER_ID='" & Web.HttpContext.Current.Session("UserId") & "'"
        '    Dim tDS As DataSet = objDb.FillDs(lsSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        SSO = tDS.Tables(0).Rows(0).Item(0)
        '    End If
        'End Function

        'Public Function Login() As Boolean
        '    lsSql = "UPDATE USERS_MSTR SET UM_LOGOUT='N' WHERE UM_USER_ID='" & Web.HttpContext.Current.Session("UserId") & "'"
        '    objDb.Execute(lsSql)
        'End Function

        'Public Function Logout() As Boolean
        '    Dim objDb1 As New EAD.DBCom
        '    lsSql = "UPDATE USERS_MSTR SET UM_LOGOUT='Y' WHERE UM_USER_ID='" & Web.HttpContext.Current.Session("UserId") & "'"
        '    objDb1.Execute(lsSql)
        'End Function

        Public Sub Log_UserActivity(ByVal userID As String, ByVal companyID As String, ByRef pQuery() As String, ByVal strModule As String, ByVal strAction As String, Optional ByVal strLookUp As String = "", Optional ByVal strRefKey As String = "", Optional ByVal strAppPkg As String = "eProcure")
            Dim strCoyID, strUserID As String

            strCoyID = userID
            strUserID = companyID

            Call Log_UserActivity_NonSession(pQuery, strCoyID, strUserID, strModule, strAction, strLookUp, strRefKey, strAppPkg)
        End Sub

        Public Sub Log_UserActivity_NonSession(ByRef pQuery() As String, ByVal strCoyID As String, ByVal strUserID As String, ByVal strModule As String, ByVal strAction As String, Optional ByVal strLookUp As String = "", Optional ByVal strRefKey As String = "", Optional ByVal strAppPkg As String = "eProcure")
            Dim strSql As String

            If strLookUp = String.Empty Then
                strLookUp = "-"
            End If

            If strRefKey = String.Empty Then
                strRefKey = "-"
            End If

            'Michelle - 12/10/2012 (To capture the time)
            strSql = "INSERT SYSTEM_AUDIT " _
            & "(SA_ACTION_DATE, SA_COY_ID,SA_USER_ID,SA_MODULE,SA_ACTION,SA_LOOKUP_KEY,SA_REF_KEY,SA_USER_IP,SA_APP_PKG,SA_SESSION_ID) VALUES(Now(), '" _
            & Common.Parse(strCoyID) & "','" & Common.Parse(strUserID) & "','" & Common.Parse(strModule) & "','" _
            & Common.Parse(strAction) & "','" & Common.Parse(strLookUp) & "','" & Common.Parse(strRefKey) & "','" _
            & Common.Parse("AgoraNEO") & "','" & Common.Parse(strAppPkg) & "','" & Common.Parse("AgoraNEO") & "')"

            Common.Insert2Ary(pQuery, strSql)
        End Sub

        Public Sub Log_UserActivityNew(ByVal strCoyID As String, ByVal strUserID As String, ByRef pQuery() As String, ByVal strModule As String, ByVal strAction As String, Optional ByVal strLookUp As String = "", Optional ByVal strRefKey As String = "", Optional ByVal strAppPkg As String = "eProcure")

            Call Log_UserActivity_NonSessionNew(pQuery, strCoyID, strUserID, strModule, strAction, strLookUp, strRefKey, strAppPkg)
        End Sub

        Public Sub Log_UserActivity_NonSessionNew(ByRef pQuery() As String, ByVal strCoyID As String, ByVal strUserID As String, ByVal strModule As String, ByVal strAction As String, Optional ByVal strLookUp As String = "", Optional ByVal strRefKey As String = "", Optional ByVal strAppPkg As String = "eProcure")
            Dim strSql As String

            If strLookUp = String.Empty Then
                strLookUp = "-"
            End If

            If strRefKey = String.Empty Then
                strRefKey = "-"
            End If
            'RENDIKA USERHOSTADDRESS
            Dim userHost As String = "-"
            'end add 

            ' Michelle
            strSql = "INSERT SYSTEM_AUDIT " _
            & "(SA_ACTION_DATE, SA_COY_ID,SA_USER_ID,SA_MODULE,SA_ACTION,SA_LOOKUP_KEY,SA_REF_KEY,SA_USER_IP,SA_APP_PKG,SA_SESSION_ID) VALUES(Now(), '" _
            & Common.Parse(strCoyID) & "','" & Common.Parse(strUserID) & "','" & Common.Parse(strModule) & "','" _
            & Common.Parse(strAction) & "'," & strLookUp & ",'" & Common.Parse(strRefKey) & "','" _
            & userHost & "','" & Common.Parse(strAppPkg) & "','" & Common.Parse(strUserID) & "')"


            Common.Insert2Ary(pQuery, strSql)
        End Sub

        ''Name       : EncryptString
        ''Author     : Kaithim
        ''Descption  : Encrypt algorithm
        ''Remark     : 
        ''ReturnValue: Encrypted String or Decrypted string 
        ''LastUpadte : 19 Sep 2002
        ''Version    : 1.00
        'Public Function EncryptString(ByVal strInput As String, ByVal strKey As String) As String
        '    Dim lngCount As Long, lngPtr As Long
        '    For lngCount = 1 To Len(strInput)
        '        Mid$(strInput, lngCount, 1) = Chr((Asc(Mid$(strInput, lngCount, 1))) Xor (Asc(Mid$(strKey, lngPtr + 1, 1))))
        '        lngPtr = ((lngPtr + 1) Mod Len(strKey))
        '    Next lngCount
        '    EncryptString = strInput
        'End Function

        ''Name       : UpdatePwd
        ''Author     : kk
        ''Descption  : Update user password and other info (use in forgot password). This will update teh 
        ''             Challege phase
        ''LastUpadte : 11 Dec 2004
        'Public Function UpdatePwd(ByVal pUserId As String, ByVal pCompId As String,
        '                          ByVal pPwd As String, ByVal pQ As Integer,
        '                          ByVal pAns As String) As Boolean
        '    Dim strSQL As String
        '    Dim objComp As New Companies
        '    Dim iPwdDuration As Integer
        '    iPwdDuration = objComp.GetPwdDuration(pCompId)
        '    If iPwdDuration = 0 Then
        '        iPwdDuration = 7
        '    End If

        '    pPwd = Common.EncryptString(pPwd)

        '    strSQL = "UPDATE USER_MSTR SET UM_PASSWORD = CAST('" & pPwd & "' AS BINARY), " &
        '                 "UM_NEXT_EXPIRE_DT =" & Common.ConvertDate(DateAdd(DateInterval.Day, iPwdDuration - 1, Today)) & ", " &
        '                 "UM_NEW_PASSWORD_IND='N', " &
        '                 "UM_QUESTION='" & pQ & "', " &
        '                 "UM_ANSWER='" & Common.Parse(pAns) & "' " &
        '                 "WHERE UM_USER_ID = '" & pUserId & "' AND UM_COY_ID='" & pCompId & "'"

        '    If objDb.Execute(strSQL) Then
        '        UpdatePasswordHistory(pUserId, pCompId, pPwd)
        '        strUserMassage = Common.RecordSave
        '        Return True
        '    Else
        '        strUserMassage = Common.RecordNotSave
        '        Return False
        '    End If
        'End Function

        'Name       : UpdateUserDetails
        'Author     : kk
        'Descption  : 
        'Remark     : pNewUser = true mean password duration will get NEW_PASSWORD_DURATION,
        '             pNewUser = false mean password duration will get Company pwd duration,
        '             ONLY USER CHG PWD BY THENSELF have to use company pwd duration 
        'ReturnValue: Update user login status after succesuful login
        'LastUpadte : 22 Nox 2004
        'Version    : 1.00
        Public Function UpdatePassword(ByVal pUserID As String, ByVal pNewPwd As String, ByVal pCompId As String, ByVal pNewUser As Boolean) As Boolean
            Dim strSQL, strCompId As String
            Dim objUser As New Users


            'pNewPwd = objUser.EncryptString(pNewPwd, "[+-*/]")
            pNewPwd = Common.EncryptString(pNewPwd)
            If pNewUser Then
                'Set new password indicator to New password
                strSQL = "UPDATE USER_MSTR SET UM_NEW_PASSWORD_IND='Y', UM_DELETED='N',UM_NEXT_EXPIRE_DT=" & Common.ConvertDate(GetPwdExpireDt(pCompId, pNewUser)) & ",UM_PASSWORD= CAST('" & pNewPwd &
                         "' AS BINARY) WHERE UM_USER_ID='" & pUserID & "' AND UM_COY_ID='" & pCompId & "'"
            Else
                'Did not set ew password indicator to new password
                strSQL = "UPDATE USER_MSTR SET UM_NEXT_EXPIRE_DT=" & Common.ConvertDate(GetPwdExpireDt(pCompId, pNewUser)) & ",UM_PASSWORD= CAST('" & pNewPwd &
                         "' AS BINARY) WHERE UM_USER_ID='" & pUserID & "' AND UM_COY_ID='" & pCompId & "'"
            End If

            If objDb.Execute(strSQL) Then
                UpdatePasswordHistory(pUserID, pCompId, pNewPwd)
                strUserMassage = Common.RecordSave
                Return True
            Else
                strUserMassage = Common.RecordNotSave
                Return False
            End If
        End Function

        'Name       : GetPwdExpireDt
        'Author     : kk
        'Descption  : 
        'Remark     : 
        'ReturnValue: Get next expire date for password 
        'LastUpadte : 22 Nox 2004
        'Version    : 1.00
        Public Function GetPwdExpireDt(ByVal pCompId As String, ByVal pNewUser As Boolean) As String
            Dim iNewPwdSpan As Integer
            Dim objLPolicy As New LoginPolicy
            Dim objComp As New Companies

            If pNewUser Then
                iNewPwdSpan = objLPolicy.getParamValue("NEW_PASSWORD_DURATION")
            Else
                iNewPwdSpan = objComp.GetPwdDuration(pCompId)
            End If

            If iNewPwdSpan = Nothing Then
                iNewPwdSpan = 7
            End If

            Return DateAdd(DateInterval.Day, iNewPwdSpan - 1, Today)

        End Function

        'Public Function GetAllUserGrp() As DataView
        '    Dim strGet As String
        '    Dim dvUserGrp As DataView
        '    strGet = "SELECT * USER_GROUP_MSTR"
        '    dvUserGrp = objDb.GetView(strGet)
        '    GetAllUserGrp = dvUserGrp
        'End Function
        ''****************************meilai 12/1/2005 get User License******************
        ''Public Function getUserLicense() As DataSet
        ''    'Dim objDb As New EAD.DBCom
        ''    Dim strGet As String
        ''    Dim dsParam As New DataSet
        ''    strGet = "select cm_license_users from company_mstr where cm_coy_id='" & HttpContext.Current.Session("CompanyId") & "'"
        ''    dsParam = objDb.FillDs(strGet)
        ''    getUserLicense = dsParam

        'Public Function getUserLicense(ByVal pHubLevel As Boolean) As String
        '    Dim strComp As String
        '    Dim strSQL As String
        '    If pHubLevel Then
        '        strComp = ctx.Session("CompanyIdToken")
        '    Else
        '        strComp = ctx.Session("CompanyId")
        '    End If

        '    strSQL = "SELECT CM_LICENSE_USERS FROM COMPANY_MSTR " &
        '            " WHERE CM_COY_ID='" & Common.Parse(strComp) & "' AND CM_DELETED<>'Y'"

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        getUserLicense = tDS.Tables(0).Rows(j).Item("cm_license_users").ToString.Trim
        '    Next
        'End Function

        ''*********************meilai 17/1/2005 check the Company Type**********************
        'Public Function getCompanyType() As String
        '    Dim strComp As String
        '    Dim strSQL As String

        '    strSQL = "select cm_coy_type from company_mstr where cm_coy_id='" & HttpContext.Current.Session("CompanyId") & "'"

        '    Dim tDS As DataSet = objDb.FillDs(strSQL)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        getCompanyType = tDS.Tables(0).Rows(j).Item("cm_coy_type").ToString.Trim
        '    Next
        'End Function

        ''*******************meilai 13/1/2005 get total numbers of active user**********************
        'Public Function getActiveUser(ByVal pHubLevel As Boolean, Optional ByVal pCountSuperAdmin As Boolean = False) As String

        '    Dim strComp As String
        '    If pHubLevel Then
        '        strComp = ctx.Session("CompanyIdToken")
        '    Else
        '        strComp = ctx.Session("CompanyId")
        '    End If

        '    Dim strsql As String
        '    strsql = "SELECT COUNT(*)AS activeuser FROM USER_MSTR WHERE UM_DELETED<>'Y' AND UM_STATUS='A' " &
        '            "AND UM_COY_ID='" & Common.Parse(strComp) & "'"

        '    If Not pCountSuperAdmin Then
        '        strsql &= " AND UM_USER_ID NOT IN (" &
        '                " SELECT UU_USER_ID FROM USERS_USRGRP, USER_GROUP_MSTR WHERE " &
        '                " UU_USRGRP_ID = UGM_USRGRP_ID" &
        '                " AND UU_APP_PKG = UGM_APP_PKG" &
        '                " AND UU_COY_ID = UM_COY_ID" &
        '                " AND UGM_FIXED_ROLE = '" & FixedRole.Super_Admin.ToString.Replace("_", " ") & "'" &
        '                ")"
        '    End If

        '    Dim tDS As DataSet = objDb.FillDs(strsql)
        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        getActiveUser = tDS.Tables(0).Rows(j).Item("activeuser").ToString.Trim

        '    Next
        'End Function

        ''**********************meilai 18/1/2005 to get email address regarding to the role selected******
        'Public Function SendMassMail(ByVal pRole As FixedRole, ByVal sendFrom As String, ByVal sendTo As String, ByVal subject As String, ByVal content As String, Optional ByVal Attachment As ArrayList = Nothing) As String
        '    Dim strRole As String

        '    Dim objMail As New AppMail
        '    strRole = System.Enum.GetName(GetType(FixedRole), pRole)
        '    'Dim strFrom, strSubject, strBody As String


        '    Dim strSQL As String
        '    Dim tDS As DataSet
        '    Dim strTo As String = ""
        '    Dim TotalEmail As String = ""

        '    ''strSQL = "SELECT * FROM USER_GROUP_MSTR WHERE UGM_USRGRP_ID IN ('" & pUserGroup & "') AND UGM_FIXED_ROLE= '" & Replace(strRole, "_", " ") & "'"
        '    'strSQL = "select distinct um_email from user_mstr, users_usrgrp, user_group_mstr, company_mstr where cm_coy_type<>'Hub' and um_deleted<>'Y' and um_status='A' and ugm_fixed_role='" & Replace(strRole, "_", " ") & "' and um_status<>'N' and cm_deleted<>'Y' and cm_status='A' and ugm_usrgrp_id=uu_usrgrp_id and uu_coy_id=cm_coy_id and um_user_id = uu_user_id And um_coy_id = uu_coy_id"
        '    'rAppCommon = objDb.GetRAppCommoner(strSQL)
        '    'Do While rRAppCommon
        '    '    strTo = rAppCommon("um_email").ToString.Trim & ","
        '    'Loop
        '    'rClose()
        '    Dim strCoy As String = ""
        '    objMail.MailFrom = sendFrom
        '    objMail.Subject = subject
        '    objMail.Body = content
        '    objMail.Attachment = Attachment

        '    If sendTo = "" Then
        '        ' ai chu modified 18/10/2005
        '        ' cannot send all mail once coz too many recipient
        '        ' so i company 1 mail
        '        ' ai chu modified 19/10/2005
        '        ' 1 email 1 user
        '        If pRole = FixedRole.All_Role Then
        '            strSQL = "select distinct um_email,um_coy_id from user_mstr, users_usrgrp, user_group_mstr, company_mstr "
        '            strSQL &= "where cm_coy_type<>'Hub' and um_deleted<>'Y' and um_status='A' and cm_deleted<>'Y' and cm_status='A' "
        '            strSQL &= "and ugm_usrgrp_id=uu_usrgrp_id and uu_coy_id=cm_coy_id and um_user_id = uu_user_id And um_coy_id = uu_coy_id "
        '            strSQL &= "order by um_coy_id "
        '        Else
        '            strSQL = "select distinct um_email,um_coy_id from user_mstr, users_usrgrp, user_group_mstr, company_mstr "
        '            strSQL &= "where cm_coy_type<>'Hub' and um_deleted<>'Y' and um_status='A' and ugm_fixed_role='" & Replace(strRole, "_", " ") & "' "
        '            strSQL &= "and cm_deleted<>'Y' and cm_status='A' and ugm_usrgrp_id=uu_usrgrp_id "
        '            strSQL &= "and uu_coy_id=cm_coy_id and um_user_id = uu_user_id And um_coy_id = uu_coy_id "
        '            strSQL &= "order by um_coy_id "
        '        End If
        '        tDS = objDb.FillDs(strSQL)
        '        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '            If Regex.IsMatch(tDS.Tables(0).Rows(j).Item("um_email").ToString.Trim, "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") Then
        '                'If strCoy = "" Or UCase(strCoy) = UCase(rAppCommon("um_coy_id").ToString.Trim) Then
        '                '    strCoy = rAppCommon("um_coy_id").ToString.Trim
        '                '    strTo &= rAppCommon("um_email").ToString.Trim & ","
        '                'Else
        '                '    ' if different company then only send mail
        '                '    objMail.MailFrom = sendFrom
        '                '    objMail.MailBCc = strTo
        '                '    objMail.Subject = subject
        '                '    objMail.Body = content
        '                '    objMail.SendMail()

        '                '    strCoy = rAppCommon("um_coy_id").ToString.Trim
        '                '    strTo = rAppCommon("um_email").ToString.Trim & ","
        '                'End If
        '                objMail.MailTo = tDS.Tables(0).Rows(j).Item("um_email").ToString.Trim
        '                objMail.SendMail()
        '            End If

        '            'TotalEmail = TotalEmail + strTo
        '        Next

        '        'objMail.MailFrom = sendFrom
        '        'objMail.MailBCc = strTo
        '        'objMail.Subject = subject
        '        'objMail.Body = content
        '        'objMail.SendMail()
        '        'rClose()
        '    Else
        '        'strSQL = sendTo
        '        strTo = sendTo
        '        objMail.MailTo = strTo
        '        objMail.SendMail()
        '    End If

        '    'strSQL = sendFrom + strSQL + subject + content
        '    ' this is for last company email or strTo <> ""
        '    'objMail.MailFrom = sendFrom
        '    'objMail.MailBCc = strTo
        '    'objMail.Subject = subject
        '    'objMail.Body = content
        '    'objMail.SendMail()

        'End Function

        'Public Function SendMassMail(ByVal pUserGroupId As String, ByVal sendFrom As String, ByVal subject As String, ByVal content As String, Optional ByVal Attachment As ArrayList = Nothing) As String
        '    Dim objMail As New AppMail

        '    Dim strSQL As String

        '    Dim strTo As String = ""
        '    Dim TotalEmail As String = ""

        '    Dim strCoy As String = ""
        '    objMail.MailFrom = sendFrom
        '    objMail.Subject = subject
        '    objMail.Body = content
        '    objMail.Attachment = Attachment

        '    strSQL = "select distinct um_email from user_mstr, users_usrgrp, user_group_mstr, company_mstr" &
        '        " where cm_coy_type <> 'Hub' and um_deleted <> 'Y' and um_status = 'A' and cm_deleted <> 'Y' and cm_status = 'A'" &
        '        " and ugm_usrgrp_id = uu_usrgrp_id and uu_coy_id = cm_coy_id and um_user_id = uu_user_id and um_coy_id = uu_coy_id" &
        '        " and ugm_app_pkg = uu_app_pkg" &
        '        " and uu_usrgrp_id = '" & pUserGroupId & "'"
        '    Dim tDS As DataSet = objDb.FillDs(strSQL)

        '    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        If Regex.IsMatch(tDS.Tables(0).Rows(j).Item("um_email").ToString.Trim, "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") Then
        '            objMail.MailTo = tDS.Tables(0).Rows(j).Item("um_email").ToString.Trim
        '            objMail.SendMail()
        '        End If
        '    Next
        'End Function

        'Public Function SearchUserGroup(ByVal pUserGroupId As String, ByVal pUserGroupName As String) As DataSet
        '    Dim strsql_usrgrp As String
        '    Dim dsUser As DataSet
        '    Dim strTemp As String

        '    strsql_usrgrp = "SELECT * FROM USER_GROUP_MSTR WHERE  "

        '    If pUserGroupId <> "" Then
        '        strTemp = Common.BuildWildCard(pUserGroupId)
        '        strsql_usrgrp &= "  UPPER(UGM_USRGRP_ID)" & Common.ParseSQL(strTemp) & " AND "
        '    End If

        '    If pUserGroupName <> "" Then
        '        strTemp = Common.BuildWildCard(pUserGroupName)
        '        strsql_usrgrp &= "  UPPER(UGM_USRGRP_NAME)" & Common.ParseSQL(strTemp) & " AND "
        '    End If
        '    strsql_usrgrp &= " UGM_DELETED<>'Y'  ORDER BY UGM_USRGRP_ID "

        '    dsUser = objDb.FillDs(strsql_usrgrp)
        '    SearchUserGroup = dsUser
        'End Function

        ''Name       : IsBuyer
        ''Author     : Moo
        ''Descption  : To check whether login user is a buyer
        ''LastUpadte : 14 Dec 2004

        ''Michelle (18/7/2011) - To check for Buyer Fixed role
        ''Public Function IsBuyer() As Boolean
        'Public Function IsBuyer(Optional ByVal pUserID As String = "") As Boolean
        '    Dim strRole As String
        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer)
        '    strRole = "'" & Replace(strRole, "_", " ") & "'"
        '    Return checkUserFixedRole(strRole, pUserID)
        'End Function

        ''Added by Joon on 03 Oct 2011
        'Public Function IsStoreKeeper(Optional ByVal pUserID As String = "") As Boolean
        '    Dim strRole As String
        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Store_Keeper)
        '    strRole = "'" & Replace(strRole, "_", " ") & "'"
        '    Return checkUserFixedRole(strRole, pUserID)
        'End Function

        ''Added by Joon on 21 Oct 2011 (Issue 1059)
        'Public Function IsBuyerAdmin(Optional ByVal pUserID As String = "") As Boolean
        '    Dim strRole As String
        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator)
        '    strRole = "'" & Replace(strRole, "_", " ") & "'"
        '    Return checkUserFixedRole(strRole, pUserID)
        'End Function

        ''Name       : IsPurchasing
        ''Author     : Moo
        ''Descption  : To check whether login user is a Purchasing Manager
        ''LastUpadte : 14 Dec 2004
        'Public Function IsPurchasing(Optional ByVal pUserID As String = "") As Boolean
        '    Dim strRole As String
        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Manager)
        '    strRole = "'" & strRole & "','" & System.Enum.GetName(GetType(FixedRole), FixedRole.Purchasing_Officer) & "'"
        '    strRole = Replace(strRole, "_", " ")
        '    Return checkUserFixedRole(strRole, pUserID)
        'End Function

        'Public Function IsAdmin() As Boolean
        '    Dim strRole As String
        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Super_Admin)
        '    strRole = "'" & Replace(strRole, "_", " ") & "'"
        '    Return checkUserFixedRole(strRole)
        'End Function

        'Public Function IsAdmin(ByVal pUserId As String, ByVal pCompanyId As String) As Boolean
        '    Dim strSql As String
        '    Dim strRole As String

        '    strRole = System.Enum.GetName(GetType(FixedRole), FixedRole.Super_Admin)
        '    strRole = "'" & Replace(strRole, "_", " ") & "'"

        '    strSql = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
        '    "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE IN (" & strRole & ") " &
        '    "AND G.UU_USER_ID='" & pUserId & "' AND G.UU_COY_ID='" & pCompanyId & "'"

        '    If objDb.Exist(strSql) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        'Public Function IsNormalAdmin() As Boolean
        '    Dim strRole As String
        '    strRole = "'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Vendor_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Vendor_Administrator) & "'"
        '    strRole = Replace(strRole, "_", " ")
        '    Return checkUserFixedRole(strRole)
        'End Function

        'Public Function IsNormalAdmin(ByVal pUserId As String, ByVal pCompanyId As String) As Boolean
        '    Dim strSql As String
        '    Dim strRole As String

        '    strRole = "'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Vendor_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Vendor_Administrator) & "'"
        '    strRole = Replace(strRole, "_", " ")

        '    strSql = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
        '    "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE IN (" & strRole & ") " &
        '    "AND G.UU_USER_ID='" & pUserId & "' AND G.UU_COY_ID='" & pCompanyId & "'"

        '    If objDb.Exist(strSql) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        '' Created By: Tan Ai Chu
        '' Created Date: 02/05/2006
        'Public Function isOtherAppAdmin(ByVal pUserID As String, ByVal pCompanyId As String, ByVal strApp As String, ByVal strNewUserGrp As String) As Boolean
        '    Dim strsql, strsql2, strRole As String
        '    Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("SSO"))

        '    strRole = "'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Buyer_Administrator) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.RFP_Vendor_Admin) & "'"
        '    strRole = strRole & ",'" & System.Enum.GetName(GetType(FixedRole), FixedRole.Vendor_Administrator) & "'"
        '    strRole = Replace(strRole, "_", " ")

        '    strsql = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
        '    "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE IN (" & strRole & ") " &
        '    "AND G.UU_USER_ID='" & pUserID & "' AND G.UU_COY_ID='" & pCompanyId & "' " &
        '    "AND GM.UGM_APP_PKG <> '" & strApp & "' "

        '    strsql2 = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
        '    "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE IN (" & strRole & ") " &
        '    "AND GM.UGM_APP_PKG = '" & strApp & "' AND GM.UGM_USRGRP_ID = '" & strNewUserGrp & "' "

        '    If objDb.Exist(strsql) And objDb.Exist(strsql2) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        '    objDb = Nothing
        'End Function

        'Name       : checkUserFixedRole
        'Author     : Moo
        'Descption  : To check whether login user assigned to specific fixed role
        'LastUpadte : 14 Dec 2004
        Public Function checkUserFixedRole(ByVal userID As String, ByVal companyID As String, ByVal pRole As String, Optional ByVal pUserID As String = "") As Boolean
            Dim strSql As String

            'strSql = "SELECT '*' From USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " & _
            '"G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND U.UM_USER_ID = G.UU_USER_ID " & _
            '"AND GM.UGM_FIXED_ROLE IN (" & pRole & ") AND U.UM_USER_ID='" & ctx.Session("UserID") & "' AND " & _
            '"UM_COY_ID='" & ctx.Session("CompanyID") & "' AND UU_COY_ID='" & ctx.Session("CompanyID") & "'"

            strSql = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
            "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE IN (" & pRole & ") "
            'Michelle 7/1/2011
            If pUserID <> "" Then
                strSql &= "AND G.UU_USER_ID='" & Common.parseNull(pUserID) & "' AND G.UU_COY_ID='" & companyID & "'"
            Else
                strSql &= "AND G.UU_USER_ID='" & userID & "' AND G.UU_COY_ID='" & companyID & "'"
            End If
            If objDb.Exist(strSql) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function BAdminRole(ByVal pUserID As String, ByVal pCompID As String) As Boolean
            'To check where user has the Buyer Admin Role
            Dim strSql As String

            strSql = "SELECT '*' From USERS_USRGRP G, USER_GROUP_MSTR GM WHERE " &
            "G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND GM.UGM_FIXED_ROLE = 'Buyer Administrator' " &
            "AND G.UU_USER_ID='" & pUserID & "' AND G.UU_COY_ID='" & pCompID & "'"

            If objDb.Exist(strSql) Then
                Return True
            Else
                Return False
            End If
        End Function


        'Name       : getUserEmailList
        'Author     : Tanac
        'Descption  : To get user email 
        'LastUpadte : 22 Dec 2004
        'kk.remark.24122004.user fixed role as pass in para for role
        Public Function getUserEmailList(ByVal pRole As FixedRole, ByVal strCoyId As String, Optional ByVal strCondition As String = "") As DataSet
            Dim strsql As String
            Dim strRole As String
            Dim ds As New DataSet
            strRole = System.Enum.GetName(GetType(FixedRole), pRole)
            Dim SSOdbName As String
            SSOdbName = ConfigurationManager.AppSettings.Get("SSOdbName")
            SSOdbName = SSOdbName & "."

            strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & Common.parseNull(strCoyId) & "'; "
            strsql &= "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID "
            strsql &= "FROM " & SSOdbName & "USER_MSTR "
            strsql &= "LEFT JOIN " & SSOdbName & "COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' "
            strsql &= "RIGHT JOIN " & SSOdbName & "USERS_USRGRP ON UU_COY_ID = CM_COY_ID AND UU_USER_ID = UM_USER_ID "
            strsql &= "LEFT JOIN " & SSOdbName & "USER_GROUP_MSTR ON UGM_USRGRP_ID = UU_USRGRP_ID "
            strsql &= "WHERE UM_COY_ID = '" & Common.parseNull(strCoyId) & "' "
            strsql &= "AND UM_DELETED <> 'Y' " 'AND UM_STATUS = 'A' "

            If strCondition <> "" Then
                strsql &= strCondition
            Else
                strsql &= "AND UGM_FIXED_ROLE = '" & Replace(strRole, "_", " ") & "' "
            End If

            ds = objDb.FillDs(strsql)
            ds.Tables(0).TableName = "COMPANY"
            ds.Tables(1).TableName = "USERTO"
            getUserEmailList = ds
        End Function

        ''Name       : getUserEmailList
        ''Author     : kk
        ''Descption  : To get user email for single user 
        ''LastUpadte : 29 Dec 2004
        'Public Function getUserEmailList(ByVal pUser As String, ByVal pCoyId As String, Optional ByVal strCondition As String = "") As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR "
        '    strsql &= "WHERE CM_COY_ID = '" & Common.parseNull(pCoyId) & "'; "
        '    strsql &= "SELECT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID " &
        '    " FROM USER_MSTR, COMPANY_MSTR " &
        '    " WHERE UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' AND UM_DELETED <> 'Y' " &
        '    " AND UM_COY_ID = '" & Common.Parse(pCoyId) & "' AND UM_USER_ID='" & Common.Parse(pUser) & "'"

        '    If strCondition <> "" Then
        '        strsql &= strCondition
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    ds.Tables(0).TableName = "COMPANY"
        '    ds.Tables(1).TableName = "USERTO"
        '    getUserEmailList = ds
        'End Function

        ''Name       : getUserEmailList
        ''Author     : Chee Hong
        ''Descption  : To get user email list based on fixed role and user access menu/ user access right 
        ''LastUpadte : 14 Nov 2013
        'Public Function getUserEmailList(ByVal pMenu As String, ByVal pRole As FixedRole, ByVal pCoyId As String, Optional ByVal strCondition As String = "") As DataSet
        '    Dim strsql As String
        '    Dim strRole As String
        '    Dim ds As New DataSet
        '    strRole = System.Enum.GetName(GetType(FixedRole), pRole)

        '    strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR " &
        '            "WHERE CM_COY_ID = '" & Common.parseNull(pCoyId) & "'; " &
        '            "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID FROM USER_MSTR " &
        '            "LEFT JOIN COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' " &
        '            "INNER JOIN USERS_USRGRP ON CM_COY_ID = UU_COY_ID AND UM_USER_ID = UU_USER_ID " &
        '            "INNER JOIN USER_GROUP_MSTR ON UU_USRGRP_ID = UGM_USRGRP_ID " &
        '            "INNER JOIN USER_ACCESS_RIGHT ON UGM_USRGRP_ID = UAR_USRGRP_ID " &
        '            "INNER JOIN MENU_MSTR ON UAR_MENU_ID = MM_MENU_ID " &
        '            "WHERE UM_COY_ID = '" & pCoyId & "'  AND UM_DELETED <> 'Y' " &
        '            "AND UGM_FIXED_ROLE = '" & Replace(strRole, "_", " ") & "' " &
        '            "AND MM_MENU_NAME = '" & Common.Parse(pMenu) & "' "

        '    If strCondition <> "" Then
        '        strsql &= strCondition
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    ds.Tables(0).TableName = "COMPANY"
        '    ds.Tables(1).TableName = "USERTO"
        '    getUserEmailList = ds
        'End Function

        Public Function getFirstFOEmailList(ByVal pBuyerCompanyId As String, ByVal pInvoiceNumber As String, ByVal pSCompanyId As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            pInvoiceNumber = pInvoiceNumber.Replace(",", "','")

            strsql = "SELECT FA_AO, FA_A_AO FROM FINANCE_APPROVAL, INVOICE_MSTR "
            strsql &= "WHERE IM_INVOICE_INDEX = FA_INVOICE_INDEX AND FA_SEQ = 1 "
            strsql &= "AND IM_B_COY_ID = '" & Common.Parse(pBuyerCompanyId) & "' "
            strsql &= "AND IM_INVOICE_NO IN ('" & pInvoiceNumber & "') AND IM_S_COY_ID = ('" & pSCompanyId & "') "

            ds = objDb.FillDs(strsql)

            If Not ds Is Nothing Then
                If ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                    Return Nothing
                End If

                Dim dr As DataRow = ds.Tables(0).Rows(0)
                Dim strFAAO As String = Common.parseNull(dr("FA_AO"), "")
                Dim strFAAAO As String = Common.parseNull(dr("FA_A_AO"), "")
                Dim strUserIdList As String = "'" & Common.Parse(strFAAO) & "'"

                If strFAAAO <> "" Then
                    strUserIdList &= ",'" & Common.Parse(strFAAAO) & "'"
                End If

                strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR "
                strsql &= "WHERE CM_COY_ID = '" & Common.parseNull(pBuyerCompanyId) & "'; "
                strsql &= "SELECT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID " &
                " FROM USER_MSTR, COMPANY_MSTR " &
                " WHERE UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' AND UM_DELETED <> 'Y' " &
                " AND UM_COY_ID = '" & Common.Parse(pBuyerCompanyId) & "' AND UM_USER_ID IN (" & strUserIdList & ")"

                ds = objDb.FillDs(strsql)
                ds.Tables(0).TableName = "COMPANY"
                ds.Tables(1).TableName = "USERTO"

                Return ds
            End If

            Return Nothing
        End Function

        Public Function getHubEmailList() As DataSet
            'created by esther on 03/01/2006
            'for Public registration screen.  To send email to inform AppCommonmin to approve new company registration

            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = 'Hub'; "
            strsql &= "SELECT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID " &
            " FROM USER_MSTR, COMPANY_MSTR " &
            " WHERE UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' AND UM_DELETED <> 'Y' " &
            " AND UM_COY_ID = 'HUB' "

            ds = objDb.FillDs(strsql)
            ds.Tables(0).TableName = "COMPANY"
            ds.Tables(1).TableName = "USERTO"
            getHubEmailList = ds
        End Function


        'Name       : getPMEmailList
        'Author     : Tanac
        'Descption  : To get PM email 
        'LastUpadte : 7 Mac 2005
        'kk.remark.24122004.user fixed role as pass in para for role
        Public Function getPMEmailList(ByVal pRole As FixedRole, ByVal strSCoyId As String, ByVal intType As Integer, ByVal strGroupIndex As String) As DataSet
            ' intType : 0 - all
            '           1 - list price only
            '           2 - discount price only
            Dim objDbProcure As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))

            Dim strsql, strsql1, strsql2 As String
            Dim strRole As String
            Dim tds, ds As New DataSet
            Dim i As Integer
            Dim comp As String = ""

            strRole = System.Enum.GetName(GetType(FixedRole), pRole)
            strsql1 = "SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_S_COY_ID = '" & strSCoyId & "' "
            strsql2 = "SELECT CDC_B_COY_ID FROM CONTRACT_DIST_COY WHERE CDC_GROUP_INDEX IN (" & strGroupIndex & ") "
            tds = objDbProcure.FillDs(strsql1)
            For i = 0 To tds.Tables(0).Rows.Count - 1
                If i = tds.Tables(0).Rows.Count - 1 Then
                    comp &= "'" & tds.Tables(0).Rows(i).Item(0) & "'"
                Else
                    comp &= "'" & tds.Tables(0).Rows(i).Item(0) & "',"
                End If
            Next
            strsql = "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID "
            strsql &= "FROM USER_MSTR "
            strsql &= "LEFT JOIN COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' "
            strsql &= "RIGHT JOIN USERS_USRGRP ON UU_COY_ID = CM_COY_ID AND UU_USER_ID = UM_USER_ID "
            strsql &= "LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID = UU_USRGRP_ID "
            strsql &= "WHERE UM_DELETED <> 'Y' AND UM_STATUS = 'A' "
            strsql &= "AND UGM_FIXED_ROLE = '" & Replace(strRole, "_", " ") & "' "
            strsql &= "AND UM_COY_ID IN ("
            Select Case intType
                Case 0
                    strsql &= strsql1 & " UNION " & strsql2 & ") "
                Case 1
                    strsql &= comp & ") "
                Case 2
                    strsql &= strsql2 & ") "
            End Select

            ds = objDbProcure.FillDs(strsql)
            getPMEmailList = ds
        End Function

        'Name       : getUserEmailList
        'Author     : Louise
        'Descption  : To get user email 
        'LastUpadte : 25 July 2005
        'kk.remark.24122004.user fixed role as pass in para for role
        Public Function getUserEmailListRFP(ByVal pRole As FixedRole, ByVal strCoyId As String, Optional ByVal strCondition As String = "") As DataSet
            Dim strsql As String
            Dim strRole As String
            Dim ds As New DataSet
            strRole = System.Enum.GetName(GetType(FixedRole), pRole)

            strsql = "SELECT CM_COY_NAME, CM_DELETED, CM_STATUS, CM_COY_TYPE FROM COMPANY_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & Common.parseNull(strCoyId) & "'; "
            strsql &= "SELECT DISTINCT UM_USER_ID, UM_USER_NAME, UM_EMAIL, CM_COY_NAME, UM_STATUS, UM_COY_ID "
            strsql &= "FROM USER_MSTR "
            strsql &= "LEFT JOIN COMPANY_MSTR ON UM_COY_ID = CM_COY_ID AND CM_STATUS = 'A' AND CM_DELETED = 'N' "
            strsql &= "RIGHT JOIN USERS_USRGRP ON UU_COY_ID = CM_COY_ID AND UU_USER_ID = UM_USER_ID "
            strsql &= "LEFT JOIN USER_GROUP_MSTR ON UGM_USRGRP_ID = UU_USRGRP_ID "
            strsql &= "WHERE UM_COY_ID <> '" & Common.parseNull(strCoyId) & "' "
            strsql &= "AND UM_DELETED <> 'Y' " 'AND UM_STATUS = 'A' "

            If strCondition <> "" Then
                strsql &= strCondition
            Else
                strsql &= "AND UGM_FIXED_ROLE = '" & Replace(strRole, "_", " ") & "' "
            End If

            ds = objDb.FillDs(strsql)
            ds.Tables(0).TableName = "COMPANY"
            ds.Tables(1).TableName = "USERTO"
            getUserEmailListRFP = ds
        End Function

        'Name       : getUserEmailCCList
        'Author     : Tanac
        'Descption  : To get user email CC List for vendor company's vendor/vendor admin 
        'LastUpadte : 21 Dec 2005
        Public Function getUserEmailCCList(ByVal strCoyId As String, ByVal strUserId As String) As String
            Dim strsql As String
            ' For SQL2000
            'strsql &= "SELECT REPLACE(UM_EMAIL_CC,CHAR(13) + CHAR(10),',') AS CC_LIST "
            'strsql &= "FROM USER_MSTR WHERE UM_COY_ID = '" & Common.Parse(strCoyId) & "' "
            'strsql &= "AND UM_USER_ID = '" & Common.Parse(strUserId) & "' "

            ' For MySQL

            'Jules 2018.03.23 - To get rid of carriage return
            'strsql &= "SELECT REPLACE(UM_EMAIL_CC,CHAR(13),',') AS CC_LIST "
            strsql &= "SELECT REPLACE(REPLACE(UM_EMAIL_CC,CHAR(13),','),CHAR(10),'') AS CC_LIST "
            'End modification.

            strsql &= "FROM USER_MSTR WHERE UM_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strsql &= "AND UM_USER_ID = '" & Common.Parse(strUserId) & "' "

            getUserEmailCCList = objDb.GetVal(strsql)
        End Function

    End Class

    Public Class UserGroups
        Dim objDb As New EAD.DBCom
        Dim strSQL As String
        Dim strUsrGrpMassage As String
        Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Property Message() As String
            Get
                Message = strUsrGrpMassage
            End Get
            Set(ByVal Value As String)
                strUsrGrpMassage = Value
            End Set
        End Property

        Public Function GetUserGroupDetails(ByVal pUsrGrpId As String, ByVal pAppPackage As String) As UserGroup
            Dim objUsrGrp As New UserGroup
            Dim dt As DataTable

            strSQL = "SELECT * FROM USER_GROUP_MSTR WHERE " &
                " UGM_USRGRP_ID='" & Common.Parse(pUsrGrpId) & "'" &
                " AND UGM_APP_PKG='" & pAppPackage & "'"

            dt = objDb.FillDt(strSQL)

            If dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    objUsrGrp.Id = IIf(IsDBNull(.Item("UGM_USRGRP_ID")), "", .Item("UGM_USRGRP_ID"))
                    objUsrGrp.Name = IIf(IsDBNull(.Item("UGM_USRGRP_NAME")), "", .Item("UGM_USRGRP_NAME"))
                    objUsrGrp.FixedRole = IIf(IsDBNull(.Item("UGM_FIXED_ROLE")), "", .Item("UGM_FIXED_ROLE"))
                    objUsrGrp.Type = IIf(IsDBNull(.Item("UGM_TYPE")), "", .Item("UGM_TYPE"))
                    objUsrGrp.Delete = IIf(IsDBNull(.Item("UGM_DELETED")), "", .Item("UGM_DELETED"))
                    objUsrGrp.Package = IIf(IsDBNull(.Item("UGM_APP_PKG")), "", .Item("UGM_APP_PKG"))

                End With
                GetUserGroupDetails = objUsrGrp
            Else
                GetUserGroupDetails = Nothing
            End If

        End Function

        'Name       : AddUserGroup
        'Author     : kk
        'Descption  : Add new user group
        'LastUpadte : 04 Dec 2004
        Public Function AddUserGroup(ByVal pUsrGrp As UserGroup) As Boolean
            AddUserGroup = False

            Dim Query(0) As String
            Dim strSQL, strCompid As String

            'Dim objUser As New User
            strSQL = "SELECT * FROM USER_GROUP_MSTR WHERE " &
                " UGM_USRGRP_ID = '" & Common.Parse(pUsrGrp.Id) & "'" &
                " AND UGM_APP_PKG = '" & Common.Parse(pUsrGrp.Package) & "'"

            Try
                If objDb.Exist(strSQL) Then
                    strUsrGrpMassage = Common.RecordUsed
                    Return False
                Else
                    strSQL = "INSERT INTO USER_GROUP_MSTR(" &
                            "UGM_USRGRP_ID,UGM_USRGRP_NAME,UGM_DELETED," &
                            "UGM_FIXED_ROLE,UGM_TYPE,UGM_APP_PKG,UGM_ENT_BY," &
                            "UGM_ENT_DT) " &
                            "VALUES('" &
                            Common.Parse(pUsrGrp.Id) & "','" & Common.Parse(pUsrGrp.Name) & "','N','" &
                            Common.Parse(pUsrGrp.FixedRole) & "','" & Common.Parse(pUsrGrp.Type) & "','" & Common.Parse(pUsrGrp.Package) & "','" & ctx.Session("UserId") & "'," &
                            Common.ConvertDate(Now()) & ")"

                    Common.Insert2Ary(Query, strSQL)

                    If objDb.BatchExecute(Query) Then
                        strUsrGrpMassage = Common.RecordSave
                        Return True
                    End If
                End If
            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        Public Function UpdateUserGroup(ByVal pUsrGrp As UserGroup) As Boolean
            UpdateUserGroup = False

            Try

                strSQL = "UPDATE USER_GROUP_MSTR SET " &
                    "UGM_USRGRP_NAME='" & Common.Parse(pUsrGrp.Name) & "'," &
                    "UGM_FIXED_ROLE='" & Common.Parse(pUsrGrp.FixedRole) & "'," &
                    "UGM_TYPE='" & Common.Parse(pUsrGrp.Type) & "'," &
                    "UGM_MOD_BY='" & ctx.Session("UserID") & "'," &
                    "UGM_MOD_DT=" & Common.ConvertDate(Now()) &
                    " WHERE UGM_USRGRP_ID = '" & Common.Parse(pUsrGrp.Id) & "' " &
                    " AND UGM_APP_PKG = '" & Common.Parse(pUsrGrp.Package) & "'"

                If objDb.Execute(strSQL) Then
                    strUsrGrpMassage = Common.RecordSave
                    Return True
                End If

            Catch errExp As CustomException
                Common.TrwExp(errExp)
            Catch errExp1 As Exception
                Common.TrwExp(errExp1)
            End Try
        End Function

        Public Function SearchAccessRight(ByVal pUsrGrpId As String, ByVal pScreen As String, ByVal pAppPackageId As String) As DataSet
            Dim objDbAppPackage As New EAD.DBCom(ConfigurationSettings.AppSettings(pAppPackageId & "Path"))
            Dim ds As New DataSet

            'strSQL = "SELECT UAR_USRGRP_ID,MM_MENU_NAME,MM_MENU_PARENT,MM_MENU_LEVEL,MM_MENU_ID,UAR_DELETE_IND,UAR_ALLOW_INSERT,UAR_ALLOW_UPDATE,UAR_ALLOW_DELETE,UAR_ALLOW_VIEW " & _
            '         "FROM menu_mstr LEFT JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID=MM_MENU_ID AND UAR_USRGRP_ID = '" & pUsrGrpId & "' " & _
            '         "WHERE MM_GROUP = '" & pScreen & "' AND LEN(MM_MENU_LEVEL)>=3 " & _
            '         "ORDER BY MM_MENU_PARENT,MM_MENU_LEVEL "

            strSQL = " SELECT distinct UAR_USRGRP_ID,MM_MENU_NAME,MM_MENU_PARENT,MM_MENU_LEVEL,MM_MENU_ID,UAR_DELETE_IND,UAR_ALLOW_INSERT,UAR_ALLOW_UPDATE,UAR_ALLOW_DELETE,UAR_ALLOW_VIEW " & _
                        " FROM USER_GROUP_MSTR, FIXED_ROLE, MENU_MSTR LEFT JOIN USER_ACCESS_RIGHT ON UAR_MENU_ID = MM_MENU_ID AND UAR_USRGRP_ID ='" & Common.Parse(pUsrGrpId) & "'" & _
                        " WHERE UGM_FIXED_ROLE = FR_ROLE_ID AND FR_MENU_ID = MM_MENU_ID AND " & _
                        " UGM_USRGRP_ID = '" & Common.Parse(pUsrGrpId) & "'" & _
                        " AND UGM_APP_PKG = '" & Common.Parse(pAppPackageId) & "'" & _
                        " ORDER BY MM_MENU_PARENT,CAST(REPLACE(MM_MENU_LEVEL,',','') AS CHAR)"

            ds = objDbAppPackage.FillDs(strSQL)
            objDbAppPackage = Nothing

            Return ds
        End Function

        Public Function DelUserGroup(ByVal pUsrGrpId As String, ByVal pAppPackageId As String) As Boolean
            Dim Query(0) As String

            strSQL = "SELECT * FROM USERS_USRGRP WHERE " & _
                " UU_USRGRP_ID = '" & Common.Parse(pUsrGrpId) & "'" & _
                " AND UU_APP_PKG = '" & Common.Parse(pAppPackageId) & "'"

            If objDb.Exist(strSQL) = 0 Then
                strSQL = "UPDATE USER_GROUP_MSTR SET UGM_DELETED = 'Y' WHERE " & _
                    " UGM_USRGRP_ID = '" & Common.Parse(pUsrGrpId) & "'" & _
                    " AND UGM_APP_PKG = '" & Common.Parse(pAppPackageId) & "'"

                Common.Insert2Ary(Query, strSQL)

                strSQL = "UPDATE USER_ACCESS_RIGHT SET UAR_DELETE_IND = 'Y'," & _
                    " UAR_MOD_BY = '" & ctx.Session("UserId") & "', " & _
                    " UAR_MOD_DT = " & Common.ConvertDate(Now) & _
                    " WHERE UAR_USRGRP_ID = '" & Common.Parse(pUsrGrpId) & "'" & _
                    " AND UAR_APP_PKG = '" & Common.Parse(pAppPackageId) & "'"

                Common.Insert2Ary(Query, strSQL)

                If objDb.BatchExecute(Query) Then
                    strUsrGrpMassage = Common.RecordDelete
                    Return True
                Else
                    strUsrGrpMassage = Common.RecordNotSave
                    Return False
                End If
            Else
                'Existing user tie to this group
                strUsrGrpMassage = Common.RecordUsrGrpCascade
                Return False
            End If
        End Function
    End Class
End Namespace


