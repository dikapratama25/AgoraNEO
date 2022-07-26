Imports System
Imports System.Collections
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Mail
Imports System.Configuration
Imports AgoraLegacy
Imports AppGlobals
Imports System.Security.Cryptography
Imports System.Text

Namespace AgoraNEO
    Public Class Users_Ext
        Dim objDb As New EAD.DBCom
        Dim lsSql As String
        Dim strUserMassage As String
        'Dim ctx As Web.HttpContext = Web.HttpContext.Current

        Public Function GetUserStockType(ByVal pUserId As String, ByVal pCompId As String) As DataSet
            Dim strSQL As String

            strSQL = "SELECT UM_STK_TYPE_SPOT, UM_STK_TYPE_STOCK, UM_STK_TYPE_MRO FROM USER_MSTR WHERE UM_USER_ID = '" & pUserId & "' AND UM_COY_ID = '" & pCompId & "'"

            Return objDb.FillDs(strSQL)

        End Function

    End Class
End Namespace


