Imports System.Configuration
Imports System.Web
Namespace AgoraNEO
    Public Class UserRoles
        Dim strsql As String
        Dim objdb As New EAD.DBCom
        'Dim CTX As HttpContext

        Function get_UserRole(ByVal CompanyId As String, ByVal UserId As String) As String

            Dim strrole As String = ""

            strsql = "select  UU_USRGRP_ID from USERS_USRGRP where UU_COY_ID='" & CompanyId & "' AND UU_USER_ID='" & UserId & "' "
            Dim tDS As DataSet = objdb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If (strrole = "") Then
                    strrole = tDS.Tables(0).Rows(j).Item("UU_USRGRP_ID").ToString.Trim
                Else
                    strrole = strrole & "," & tDS.Tables(0).Rows(j).Item("UU_USRGRP_ID").ToString.Trim
                End If
            Next

            get_UserRole = strrole
        End Function

        Function get_UserFixedRole(ByVal CompanyId As String, ByVal UserId As String, Optional ByVal pAppPkg As String = Nothing) As String

            Dim strrole As String = ""

            strsql = "select distinct UGM_FIXED_ROLE from USER_GROUP_MSTR where UGM_DELETED='N' and UGM_USRGRP_ID in (" &
                     "select  UU_USRGRP_ID from USERS_USRGRP where UU_COY_ID='" & CompanyId & "' AND UU_USER_ID='" & UserId & "')"

            If Not pAppPkg Is Nothing Then
                strsql &= " and UGM_APP_PKG='" & pAppPkg & "'"
            End If

            Dim tDS As DataSet = objdb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If (strrole = "") Then
                    strrole = tDS.Tables(0).Rows(j).Item("UGM_FIXED_ROLE").ToString.Trim
                Else
                    strrole = strrole & "," & tDS.Tables(0).Rows(j).Item("UGM_FIXED_ROLE").ToString.Trim
                End If
            Next

            get_UserFixedRole = strrole
        End Function
        Function get_UserFixedRoleByParameter(ByVal CompanyID As String, ByVal UserID As String, Optional ByVal pAppPkg As String = Nothing) As String

            Dim strrole As String = ""

            strsql = "select distinct UGM_FIXED_ROLE from USER_GROUP_MSTR where UGM_DELETED='N' and UGM_USRGRP_ID in (" &
                     "select  UU_USRGRP_ID from USERS_USRGRP where UU_COY_ID='" & CompanyID & "' AND UU_USER_ID='" & UserID & "')"

            If Not pAppPkg Is Nothing Then
                strsql &= " and UGM_APP_PKG='" & pAppPkg & "'"
            End If

            Dim tDS As DataSet = objdb.FillDs(strsql)

            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                If (strrole = "") Then
                    strrole = tDS.Tables(0).Rows(j).Item("UGM_FIXED_ROLE").ToString.Trim
                Else
                    strrole = strrole & "," & tDS.Tables(0).Rows(j).Item("UGM_FIXED_ROLE").ToString.Trim
                End If
            Next

            Return strrole
        End Function
        Function get_DashboardUserFixedRole(ByVal UserId As String, ByVal CompanyId As String, Optional ByVal pAppPkg As String = Nothing) As DataSet
            Dim strsql As String
            strsql = "select distinct UGM_FIXED_ROLE from USER_GROUP_MSTR where UGM_DELETED='N' and UGM_USRGRP_ID in (" &
                     "select  UU_USRGRP_ID from USERS_USRGRP where UU_COY_ID='" & CompanyId & "' AND UU_USER_ID='" & UserId & "')"

            If Not pAppPkg Is Nothing Then
                strsql &= " and UGM_APP_PKG='" & pAppPkg & "'"
            End If

            Dim tDS As DataSet = objdb.FillDs(strsql)
            Return tDS
        End Function
    End Class
End Namespace

