Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports AgoraNEO
Namespace AgoraNEO
    Public Class BuyerCat
        Dim objDb As New EAD.DBCom
        'Function chkVendorState(ByVal valvendor As String)
        '    Dim strSQL As String
        '    'strSQL = "SELECT '*' FROM COMPANY_VENDOR WHERE CV_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '    '         "AND CV_S_COY_ID = '" & valvendor & "'"

        '    strSQL = "SELECT '*' FROM COMPANY_VENDOR, Company_Mstr WHERE CV_S_COY_ID =CM_COY_ID AND CV_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '           "AND CV_S_COY_ID = '" & valvendor & "'"

        '    If objDb.Exist(strSQL) > 0 Then
        '        chkVendorState = 1
        '    Else
        '        chkVendorState = 0
        '    End If

        'End Function

        'Function unavailable_buttoncontrol(ByVal favIndex As String)
        '    Dim strSQL As String = "SELECT * FROM FAVOURITE_LIST_ITEMS " & _
        '                        "WHERE FLI_S_COY_ID <> '" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                        "AND FLI_LIST_INDEX= '" & favIndex & "' " & _
        '                        "AND ( " & _
        '                        "(FLI_PRODUCT_CODE NOT IN " & _
        '                        "(SELECT DISTINCT CDI_PRODUCT_CODE FROM CONTRACT_DIST_MSTR " & _
        '                        "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX AND CDC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                        "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '                        "WHERE CDM_TYPE = 'C' " & _
        '                        "AND CONVERT(VARCHAR(10),GETDATE(),112) BETWEEN CONVERT(VARCHAR(10),CDM_START_DATE,112) " & _
        '                        "AND CONVERT(VARCHAR(10),CDM_END_DATE,112)) AND FLI_SOURCE <> 'LP') " & _
        '                        "OR " & _
        '                        "(FLI_PRODUCT_CODE NOT IN " & _
        '                        "(SELECT CDI_PRODUCT_CODE " & _
        '                        "FROM CONTRACT_DIST_ITEMS " & _
        '                        "LEFT JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDI_GROUP_INDEX " & _
        '                        "LEFT JOIN FAVOURITE_LIST_ITEMS ON FLI_CD_GROUP_INDEX = CDM_GROUP_INDEX AND FLI_LIST_INDEX = '" & favIndex & "' " & _
        '                        "WHERE CONVERT(VARCHAR(10),GETDATE(),112) < CONVERT(VARCHAR(10),CDM_END_DATE,112)) AND FLI_SOURCE <> 'LP') " & _
        '                        "OR " & _
        '                        "(FLI_S_COY_ID NOT IN " & _
        '                        "(SELECT CV_S_COY_ID FROM Company_Vendor, Company_Mstr " & _
        '                        "where CV_S_COY_ID =CM_COY_ID and CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "')))"


        '    If objDb.Exist(strSQL) > 0 Then
        '        unavailable_buttoncontrol = 1
        '    Else
        '        unavailable_buttoncontrol = 0
        '    End If
        'End Function

        'Function getindex() As Long

        '    getindex = objDb.GetMax("BUYER_CATALOGUE_MSTR", "BCM_CAT_INDEX", "WHERE BCM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "'")

        'End Function

        '//To get assigned buyer of a buyer cat
        Function bindlistbox_BuyerCatSelectedData(ByVal strCatName As String) As DataView
            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            '"SELECT C.CDM_DEPT_NAME,U.UM_USER_ID, U.UM_USER_NAME, " & _
            '"(C.CDM_DEPT_NAME + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
            ' "AND C.CDM_DEPT_CODE=U.UM_DEPT_ID " & _
            '"AND U.UM_COY_ID = C.CDM_COY_ID"
            strSql = "SELECT distinct U.UM_USER_ID, U.UM_USER_NAME, " &
                         objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " &
                         "FROM BUYER_CATALOGUE_USER K, USER_MSTR U " &
                         "where k.BCU_CAT_INDEX = '" & strCatName & "' " &
                         "AND K.BCU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " &
                         "AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A'"
            'Michelle (22/10/2010) 

            '"FROM BUYER_CATALOGUE_USER K, USER_MSTR U,COMPANY_DEPT_MSTR C " & _

            strSql &= " ORDER BY U.UM_USER_NAME"

            drw = objDB.GetView(strSql)
            Return drw
        End Function
        '//To get those buyers that not assigned to a buyer cat
        'Function bindlistbox_BuyerCatSearchData(ByVal strCatName As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom
        '    '"SELECT C.CDM_DEPT_NAME,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '    '"(C.CDM_DEPT_NAME + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '    strSql = "SELECT  distinct U.UM_USER_ID, U.UM_USER_NAME, " & _
        '             objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " & _
        '             "FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM " & _
        '             "where UM_USER_ID NOT IN (SELECT K.BCU_USER_ID from BUYER_CATALOGUE_MSTR B ,BUYER_CATALOGUE_USER K " & _
        '             "where B.BCM_CAT_INDEX = k.BCU_CAT_INDEX AND B.BCM_CAT_INDEX = '" & strCatName & "' ) " & _
        '             "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A' " & _
        '             "AND G.UU_USRGRP_ID = GM.UGM_USRGRP_ID " & _
        '             "AND (GM.UGM_FIXED_ROLE ='Buyer' OR GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR GM.UGM_FIXED_ROLE= 'Purchasing Officer') " & _
        '             "AND UM_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' ORDER BY U.UM_USER_NAME"
        '    '"FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM,COMPANY_DEPT_MSTR C " & _

        '    '"AND C.CDM_DEPT_CODE=U.UM_DEPT_ID " & _
        '    '"AND U.UM_COY_ID = C.CDM_COY_ID"
        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function
        'Function DelBuyercatAsg(ByVal strCatName As String)
        '    Dim strSQL As String
        '    Dim query(0) As String
        '    strSQL = "Delete FROM BUYER_CATALOGUE_USER where BCU_CAT_INDEX='" & strCatName & "'"
        '    Common.Insert2Ary(query, strSQL)
        '    objDb.BatchExecute(query)
        'End Function
        'Function AddBuyerCatAsgBuyer(ByVal strCatName As String, ByVal strli As String)
        '    Dim strSQL As String
        '    Dim query(0) As String
        '    strSQL = "INSERT INTO BUYER_CATALOGUE_USER (BCU_CAT_INDEX, BCU_USER_ID )VALUES('" & strCatName & "','" & strli & "')"
        '    Common.Insert2Ary(query, strSQL)
        '    objDb.BatchExecute(query)
        'End Function
        'Michelle (27/10/2010) - To add BIM into Buyer Catalogue
        'Function addBuyerCatItem(ByVal dt As DataTable) As Object
        '    Dim strSQL As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dt.Rows.Count - 1
        '        strSQL = "Insert into BUYER_CATALOGUE_ITEMS "
        '        strSQL &= "(BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
        '        strSQL &= "VALUES "
        '        strSQL &= "('" & dt.Rows(i)("index") & "', '" & dt.Rows(i)("productcode") & "', 'LP', '"
        '        strSQL &= dt.Rows(i)("bcoyid") & "', '" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now) & ")"
        '        Common.Insert2Ary(strAryQuery, strSQL)
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            addBuyerCatItem = WheelMsgNum.Save
        '        Else
        '            addBuyerCatItem = WheelMsgNum.NotSave
        '        End If
        '    End If
        'End Function
        'Function modbuyercatmain(ByVal strbcmindex As String, ByVal bcmgroupcode As String, ByVal bcmgroupdesc As String, Optional ByVal strOld As String = "", Optional ByVal strOld2 As String = "") As Integer
        '    Dim strsql, strUserID As String
        '    strUserID = HttpContext.Current.Session("UserId")

        '    'Michelle (22/10/2010) - Use Desc as the key
        '    'If UCase(strOld) <> UCase(bcmgroupcode) Then
        '    If UCase(strOld2) <> UCase(bcmgroupdesc) Then
        '        'If objDb.Exist("Select '*' From BUYER_CATALOGUE_MSTR where BCM_GRP_CODE ='" & _
        '        '               Common.Parse(bcmgroupcode) & "' AND BCM_B_COY_ID='" & _
        '        If objDb.Exist("Select '*' From BUYER_CATALOGUE_MSTR where BCM_GRP_DESC ='" & _
        '                       Common.Parse(bcmgroupdesc) & "' AND BCM_B_COY_ID='" & _
        '                       HttpContext.Current.Current.Session("CompanyID") & "'") > 0 Then
        '            modbuyercatmain = WheelMsgNum.Duplicate
        '            Exit Function
        '        End If
        '    End If
        '    '//reamrk by Moo
        '    'If strOld2 <> bcmgroupdesc Then
        '    '    If objDb.Exist("Select '*' From BUYER_CATALOGUE_MSTR where BCM_CAT_INDEX = '" & _
        '    '                   strbcmindex & "' AND BCM_GRP_CODE ='" & _
        '    '                   bcmgroupcode & "' AND BCM_GRP_DESC ='" & bcmgroupdesc & "' AND BCM_B_COY_ID='" & _
        '    '                   HttpContext.Current.Current.Session("CompanyID") & "'") > 0 Then
        '    '        modbuyercatmain = WheelMsgNum.Duplicate
        '    '        Exit Function
        '    '    End If
        '    'End If
        '    strsql = "UPDATE BUYER_CATALOGUE_MSTR SET "
        '    strsql &= "BCM_GRP_CODE = '" & Common.Parse(bcmgroupcode) & "', BCM_GRP_DESC = '" & Common.Parse(bcmgroupdesc) & "', BCM_MOD_BY= '" & strUserID & "', BCM_MOD_DATETIME=" & Common.ConvertDate(Now) & " "
        '    strsql &= "WHERE BCM_CAT_INDEX ='" & Common.Parse(strbcmindex) & "' "
        '    strsql &= "AND BCM_B_COY_ID= '" & HttpContext.Current.Current.Session("CompanyID") & "' "
        '    If objDb.Execute(strsql) Then
        '        modbuyercatmain = WheelMsgNum.Save
        '    Else
        '        modbuyercatmain = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Function addbuyercatmain(ByVal bcmgroupcode As String, ByVal bcmgroupdesc As String) As Integer
        '    Dim strsql As String
        '    Dim strCoyID, strUserID As String
        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strUserID = HttpContext.Current.Session("UserId")
        '    'Michelle (22/10/2010) - BCM_B_COY_ID + BCM_GRP_DESC will be unique
        '    'strsql = "Select '*' From BUYER_CATALOGUE_MSTR Where BCM_B_COY_ID='" & strCoyID & "' AND BCM_GRP_CODE='" & Common.Parse(bcmgroupcode) & "'"
        '    strsql = "Select '*' From BUYER_CATALOGUE_MSTR Where BCM_B_COY_ID='" & strCoyID & "' AND BCM_GRP_DESC='" & Common.Parse(bcmgroupdesc) & "'"
        '    If objDb.Exist(strsql) > 0 Then
        '        addbuyercatmain = WheelMsgNum.Duplicate
        '        Exit Function
        '    Else
        '        strsql = "INSERT INTO BUYER_CATALOGUE_MSTR (BCM_GRP_CODE,BCM_GRP_DESC,BCM_B_COY_ID, BCM_ENT_BY, BCM_ENT_DATETIME) values ('" & Common.Parse(bcmgroupcode) & "','" & Common.Parse(bcmgroupdesc) & "', '" & HttpContext.Current.Current.Session("CompanyID") & "', '" & strUserID & "', " & Common.ConvertDate(Now) & ")"

        '        If objDb.Execute(strsql) Then
        '            addbuyercatmain = WheelMsgNum.Save
        '        Else
        '            addbuyercatmain = WheelMsgNum.NotSave
        '        End If
        '    End If
        'End Function
        '//to get buyer cat of a company
        'Michelle (22/10/2010) - Base on the Buyer Catalogue
        'Function getbuyercatmain(ByVal bcmgroupcode As String, ByVal bcmgroupdesc As String) As DataSet
        '    Dim strget, strget1, strget2 As String
        '    Dim dsbuyercatmain As DataSet

        '    '//originally link with BUYER_CATALOGUE_USER to get buyer
        '    '//take out
        '    strget1 = "Select M.BCM_CAT_INDEX,M.BCM_GRP_CODE,M.BCM_GRP_DESC, 1 AS SEQ " & _
        '             "FROM BUYER_CATALOGUE_MSTR M " & _
        '             "where M.BCM_B_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and M.BCM_GRP_DESC = 'Default Purchaser Catalogue' "
        '    '"AND M.BCM_STATUS <> 'Y'"

        '    If bcmgroupcode <> "" Then
        '        strget1 = strget1 & "AND M.BCM_GRP_CODE " & Common.ParseSQL(bcmgroupcode) & ""
        '    End If
        '    If bcmgroupdesc <> "" Then
        '        strget1 = strget1 & "AND M.BCM_GRP_DESC " & Common.ParseSQL(bcmgroupdesc) & ""
        '    End If

        '    strget2 = "Select M.BCM_CAT_INDEX,M.BCM_GRP_CODE,M.BCM_GRP_DESC, 2 AS SEQ " & _
        '             "FROM BUYER_CATALOGUE_MSTR M " & _
        '             "where M.BCM_B_COY_ID ='" & HttpContext.Current.Current.Session("CompanyID") & "' and M.BCM_GRP_DESC <> 'Default Purchaser Catalogue' "

        '    If bcmgroupcode <> "" Then
        '        strget2 = strget2 & "AND M.BCM_GRP_CODE " & Common.ParseSQL(bcmgroupcode) & ""
        '    End If
        '    If bcmgroupdesc <> "" Then
        '        strget2 = strget2 & "AND M.BCM_GRP_DESC " & Common.ParseSQL(bcmgroupdesc) & ""
        '    End If

        '    strget = strget1 & " UNION " & strget2 & " ORDER BY SEQ, BCM_GRP_DESC "
        '    dsbuyercatmain = objDb.FillDs(strget)
        '    getbuyercatmain = dsbuyercatmain

        'End Function
        'Function AddShopCart(ByVal valvencode As String, ByVal valprocode As String, ByVal valprodesc As String, ByVal valunitcost As String, ByVal valuom As String, ByVal valcurrency As String)
        '    Dim strCoyId, strUserID, strSQL, strSqlAry(0) As String
        '    strCoyId = HttpContext.Current.Session("CompanyId")
        '    strUserID = HttpContext.Current.Session("UserId")
        '    strSQL = "SELECT '*' FROM SHOPPING_CART WHERE SC_USER_ID='" & strUserID & _
        '    "' AND SC_B_COY_ID='" & strCoyId & _
        '    "' AND SC_VENDOR_ITEM_CODE='" & valvencode & "'"
        '    If objDb.Exist(strSQL) > 0 Then
        '        AddShopCart = WheelMsgNum.Duplicate
        '    Else
        '        strSQL = "INSERT INTO SHOPPING_CART(SC_B_COY_ID,SC_USER_ID,SC_S_COY_ID,SC_PRODUCT_CODE, " & _
        '                 "SC_VENDOR_ITEM_CODE,SC_PRODUCT_DESC,SC_UNIT_COST, SC_QUANTITY, SC_CURRENCY_CODE, SC_UOM) " & _
        '                 " VALUES('" & Common.Parse(strCoyId) & "','" & Common.Parse(strUserID) & "', " & _
        '                 "'" & strCoyId & "','" & valprocode & "', " & _
        '                 "'" & valvencode & "','" & valprodesc & "', " & _
        '                 "" & valunitcost & ",'1','" & valcurrency & "','" & valuom & "')"
        '        Common.Insert2Ary(strSqlAry, strSQL)
        '        'objDb.Execute(strSQL)
        '        AddShopCart = WheelMsgNum.Save
        '    End If
        '    If strSqlAry(0) <> String.Empty Then
        '        objDb.BatchExecute(strSqlAry)
        '        AddShopCart = WheelMsgNum.Save
        '    End If
        '    'objDb = Nothing
        'End Function
        ''//to get items assigned to a buyer cat
        'Function getBuyerItemCat(ByVal value As String, ByVal strcode As String, ByVal strdesc As String)
        '    Dim strgetItem As String
        '    Dim dsbuyerItem As DataSet
        '    strgetItem = "SELECT B.BCU_SOURCE, B.BCU_PRODUCT_CODE, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '                 "P.PM_CATEGORY_NAME, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST " & _
        '                 "FROM BUYER_CATALOGUE_ITEMS B LEFT OUTER JOIN " & _
        '                 "PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE LEFT OUTER JOIN " & _
        '                 "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '                 "where P.PM_PRODUCT_CODE = B.BCU_PRODUCT_CODE " & _
        '                 "AND B.BCU_CAT_INDEX = '" & value & "'"

        '    If strcode <> "" Then
        '        strgetItem = strgetItem & "AND B.BCU_PRODUCT_CODE" & Common.ParseSQL(strcode)
        '    End If

        '    If strdesc <> "" Then
        '        strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '    End If
        '    dsbuyerItem = objDb.FillDs(strgetItem)
        '    getBuyerItemCat = dsbuyerItem
        'End Function

        'Function delbuyercatmain(ByVal strbcmindex As String)
        '    Dim strdel As String
        '    'strdel = "UPDATE BUYER_CATALOGUE_MSTR set BCM_STATUS ='Y' where BCM_CAT_INDEX = '" & strbcmindex & "' "
        '    strdel = "DELETE FROM BUYER_CATALOGUE_MSTR where BCM_CAT_INDEX = '" & strbcmindex & "' "

        '    objDb.Execute(strdel)
        '    If objDb.Execute(strdel) Then
        '        delbuyercatmain = WheelMsgNum.Delete
        '    Else
        '        delbuyercatmain = WheelMsgNum.NotDelete
        '    End If
        'End Function

        'Function delFav1(ByVal value2 As String, ByVal procode As String, ByVal soucode As String)
        '    Dim strdelFav As String
        '    strdelFav = "Delete from FAVOURITE_LIST_ITEMS where FLI_LIST_INDEX = '" & value2 & "' " & _
        '                 "AND FLI_PRODUCT_CODE = '" & procode & "' AND FLI_SOURCE = '" & soucode & "'"
        '    objDb.Execute(strdelFav)
        '    If objDb.Execute(strdelFav) Then
        '        delFav1 = WheelMsgNum.Delete
        '    Else
        '        delFav1 = WheelMsgNum.NotDelete
        '    End If

        'End Function

        ' ai chu modified on 15/12/2005
        ' need to remove items from deactivated vendor company
        'Function del_All_UnavailableFav(ByVal favIndex As String)
        '    Dim StrDelAll As String
        '    'StrDelAll = "DELETE FROM FAVOURITE_LIST_ITEMS " & _
        '    '            "WHERE FLI_S_COY_ID <> '" & HttpContext.Current.Session("CompanyID") & "' AND FLI_LIST_INDEX= '" & favIndex & "' AND " & _
        '    '            "(FLI_PRODUCT_CODE NOT IN " & _
        '    '            "(SELECT DISTINCT CDI_PRODUCT_CODE FROM CONTRACT_DIST_MSTR " & _
        '    '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' " & _
        '    '            "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '    '            "WHERE CDM_TYPE = 'C' " & _
        '    '            "AND CONVERT(VARCHAR(10),GETDATE(),112) " & _
        '    '            "BETWEEN CONVERT(VARCHAR(10),CDM_START_DATE,112) AND CONVERT(VARCHAR(10),CDM_END_DATE,112)) " & _
        '    '            "OR (FLI_S_COY_ID NOT IN " & _
        '    '            "(SELECT CV_S_COY_ID FROM Company_Vendor, Company_Mstr where CV_S_COY_ID =CM_COY_ID and CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "')))"

        '    'StrDelAll = "DELETE FROM FAVOURITE_LIST_ITEMS " & _
        '    '            "WHERE FLI_S_COY_ID <> '" & HttpContext.Current.Session("CompanyID") & "' " & _
        '    '            "AND FLI_LIST_INDEX= '" & favIndex & "' " & _
        '    '            "AND (FLI_PRODUCT_CODE NOT IN " & _
        '    '            "(SELECT DISTINCT CDI_PRODUCT_CODE " & _
        '    '            "FROM CONTRACT_DIST_MSTR " & _
        '    '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX AND CDC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '    '            "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '    '            "WHERE CDM_TYPE = 'C' " & _
        '    '            "AND CONVERT(VARCHAR(10),GETDATE(),112) BETWEEN CONVERT(VARCHAR(10),CDM_START_DATE,112) " & _
        '    '            "AND CONVERT(VARCHAR(10),CDM_END_DATE,112)) " & _
        '    '            "OR (FLI_PRODUCT_CODE NOT IN " & _
        '    '            "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '    '            "LEFT JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDI_GROUP_INDEX " & _
        '    '            "LEFT JOIN FAVOURITE_LIST_ITEMS ON FLI_CD_GROUP_INDEX = CDM_GROUP_INDEX AND FLI_LIST_INDEX = '" & favIndex & "' " & _
        '    '            "WHERE CONVERT(VARCHAR(10),GETDATE(),112) < CONVERT(VARCHAR(10),CDM_END_DATE,112)) " & _
        '    '            "OR (FLI_S_COY_ID NOT IN " & _
        '    '            "(SELECT CV_S_COY_ID FROM Company_Vendor, Company_Mstr " & _
        '    '            "where CV_S_COY_ID =CM_COY_ID and CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'))))"

        '    StrDelAll = "DELETE FROM FAVOURITE_LIST_ITEMS " & _
        '                "WHERE FLI_S_COY_ID <> '" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                "AND FLI_LIST_INDEX= '" & favIndex & "' " & _
        '                "AND ( " & _
        '                "(FLI_PRODUCT_CODE NOT IN " & _
        '                "(SELECT DISTINCT CDI_PRODUCT_CODE FROM CONTRACT_DIST_MSTR " & _
        '                "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX AND CDC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '                "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '                "WHERE CDM_TYPE = 'C' " & _
        '                "AND CONVERT(VARCHAR(10),GETDATE(),112) BETWEEN CONVERT(VARCHAR(10),CDM_START_DATE,112) " & _
        '                "AND CONVERT(VARCHAR(10),CDM_END_DATE,112)) AND FLI_SOURCE <> 'LP') " & _
        '                "OR " & _
        '                "(FLI_PRODUCT_CODE NOT IN " & _
        '                "(SELECT CDI_PRODUCT_CODE " & _
        '                "FROM CONTRACT_DIST_ITEMS " & _
        '                "LEFT JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDI_GROUP_INDEX " & _
        '                "LEFT JOIN FAVOURITE_LIST_ITEMS ON FLI_CD_GROUP_INDEX = CDM_GROUP_INDEX AND FLI_LIST_INDEX = '" & favIndex & "' " & _
        '                "WHERE CONVERT(VARCHAR(10),GETDATE(),112) < CONVERT(VARCHAR(10),CDM_END_DATE,112)) AND FLI_SOURCE <> 'LP') " & _
        '                "OR " & _
        '                "(FLI_S_COY_ID NOT IN " & _
        '                "(SELECT CV_S_COY_ID FROM Company_Vendor, Company_Mstr " & _
        '                "where CV_S_COY_ID =CM_COY_ID and CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "')) " & _
        '                "OR " & _
        '                "(FLI_S_COY_ID IN (SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_DELETED <> 'Y' AND CM_STATUS = 'I')))"

        '    If objDb.Execute(StrDelAll) Then
        '        del_All_UnavailableFav = WheelMsgNum.Delete
        '    Else
        '        del_All_UnavailableFav = WheelMsgNum.NotDelete
        '    End If
        'End Function

        ' ai chu modified on 08/11/2005
        ' to delete the items using batch execute instead of by record
        'Function delBuyerItem(ByVal value As String, ByVal procode As String, ByVal soucode As String)
        'Function delBuyerItem(ByVal dt As DataTable) As Object
        '    Dim strdelItem As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dt.Rows.Count - 1
        '        'Michelle (25/10/2010) 
        '        strdelItem = "Delete from BUYER_CATALOGUE_ITEMS where BCI_ITEM_INDEX = '" & dt.Rows(i)("index") & "' "
        '        'strdelItem = "Delete from BUYER_CATALOGUE_ITEMS where BCU_CAT_INDEX = '" & dt.Rows(i)("index") & "' "
        '        ' AND BCU_SOURCE = '" & dt.Rows(i)("source") & "'"
        '        Common.Insert2Ary(strAryQuery, strdelItem)
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            delBuyerItem = WheelMsgNum.Delete
        '        Else
        '            delBuyerItem = WheelMsgNum.NotDelete
        '        End If
        '    End If
        'End Function

        'Public Function deleteBuyerItemByGrpIndex(ByVal lngGrpIndex As Long) As Integer
        '    Dim strsql As String
        '    strsql = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_CD_GROUP_INDEX = " & lngGrpIndex
        '    If objDb.Execute(strsql) Then
        '        deleteBuyerItemByGrpIndex = WheelMsgNum.Delete
        '    Else
        '        deleteBuyerItemByGrpIndex = WheelMsgNum.NotDelete
        '    End If
        'End Function

        'Function delFavItem(ByVal valExp As String, ByVal value2 As String, ByVal procode As String, ByVal soucode As String)
        '    Dim strdelItem As String

        '    strdelItem = "Delete from FAVOURITE_LIST_ITEMS where FLI_LIST_INDEX = '" & value2 & "' " & _
        '                 "AND FLI_PRODUCT_CODE = '" & procode & "' AND FLI_SOURCE = '" & soucode & "'"
        '    objDb.Execute(strdelItem)
        '    If objDb.Execute(strdelItem) Then
        '        delFavItem = WheelMsgNum.Delete
        '    Else
        '        delFavItem = WheelMsgNum.NotDelete
        '    End If

        'End Function
        ''//to get items assigned to a Fav List
        'Function getFavCat(ByVal value2 As String)
        '    Dim strgetFav As String
        '    Dim dsFav As DataSet

        '    strgetFav = "SELECT I.FLI_SOURCE,P.PM_VENDOR_ITEM_CODE, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, I.FLI_LIST_INDEX, " & _
        '                "P.PM_CATEGORY_NAME, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, I.FLI_PRODUCT_CODE " & _
        '                "FROM FAVOURITE_LIST_ITEMS I LEFT OUTER JOIN " & _
        '                "PRODUCT_MSTR P ON I.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE LEFT OUTER JOIN " & _
        '                "COMPANY_B_ITEM_CODE C ON I.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '                "where P.PM_PRODUCT_CODE = I.FLI_PRODUCT_CODE " & _
        '                "AND I.FLI_LIST_INDEX = '" & value2 & "'"


        '    dsFav = objDb.FillDs(strgetFav)
        '    getFavCat = dsFav
        'End Function
        '//to get items assigned to a buyer cat
        'Function getBuyerCat(ByVal value1 As String)
        '    Dim strgetItem As String
        '    Dim dsbuyer As DataSet
        '    strgetItem = "SELECT B.BCU_SOURCE,P.PM_VENDOR_ITEM_CODE, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '                 "P.PM_CATEGORY_NAME, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, B.BCU_PRODUCT_CODE " & _
        '                 "FROM BUYER_CATALOGUE_ITEMS B LEFT OUTER JOIN " & _
        '                 "PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE LEFT OUTER JOIN " & _
        '                 "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '                 "where P.PM_PRODUCT_CODE = B.BCU_PRODUCT_CODE " & _
        '                 "AND B.BCU_CAT_INDEX = '" & value1 & "'"


        '    dsbuyer = objDb.FillDs(strgetItem)
        '    getBuyerCat = dsbuyer
        'End Function

        'Function getBuyerCat_CDP(ByVal value1 As String, ByVal strcode As String, ByVal strdesc As String)
        '	Dim strgetItem As String
        '	Dim dsbuyer As DataSet

        '	'strgetItem = "SELECT B.BCU_SOURCE,P.PM_VENDOR_ITEM_CODE,PM_S_COY_ID, CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '	'             "P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, B.BCU_PRODUCT_CODE, DateAdd(Year, 1000, getdate()) as CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,0) AS BCU_CD_GROUP_INDEX " & _
        '	'             "FROM BUYER_CATALOGUE_ITEMS B INNER JOIN " & _
        '	'             "PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE LEFT OUTER JOIN " & _
        '	'             "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	'             "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	'             "WHERE B.BCU_CAT_INDEX =  '" & value1 & "' AND B.BCU_SOURCE='LP' "
        '	'If strcode <> "" Then
        '	'    strgetItem = strgetItem & "AND P.PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	'End If
        '	'If strdesc <> "" Then
        '	'    strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	'End If

        '	'strgetItem &= "UNION " & _
        '	'             "SELECT B.BCU_SOURCE,DI.CDI_VENDOR_ITEM_CODE,B.BCU_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '	'             "DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	'             "DI.CDI_UNIT_COST, B.BCU_PRODUCT_CODE, D.CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,-1) AS BCU_CD_GROUP_INDEX " & _
        '	'             "FROM BUYER_CATALOGUE_ITEMS B LEFT JOIN " & _
        '	'             "CONTRACT_DIST_ITEMS DI ON B.BCU_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	'             "AND B.BCU_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	'             "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX LEFT OUTER JOIN " & _
        '	'             "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	'             "INNER JOIN COMPANY_MSTR CM ON B.BCU_S_COY_ID = CM.CM_COY_ID " & _
        '	'             "WHERE B.BCU_CAT_INDEX = '" & value1 & "' AND B.BCU_SOURCE IN ('CP','DP') "
        '	'If strcode <> "" Then
        '	'    strgetItem = strgetItem & "AND DI.CDI_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	'ElseIf strdesc <> "" Then
        '	'    strgetItem = strgetItem & "AND DI.CDI_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	'End If

        '	' ai chu modified on 09/12/2005
        '	' deactived company's item will be displayed in different color
        '          strgetItem = "SELECT B.BCU_SOURCE,P.PM_VENDOR_ITEM_CODE,PM_S_COY_ID, CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '           "P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, B.BCU_PRODUCT_CODE, DateAdd(Year,1000,getdate()) as CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,0) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '           "FROM BUYER_CATALOGUE_ITEMS B INNER JOIN " & _
        '           "PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' LEFT OUTER JOIN " & _
        '           "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '           "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '           "WHERE B.BCU_CAT_INDEX ='" & value1 & "' AND B.BCU_SOURCE='LP' AND CM.CM_DELETED<>'Y' "             'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If
        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "UNION " & _
        '	  "SELECT B.BCU_SOURCE,DI.CDI_VENDOR_ITEM_CODE,B.BCU_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '	  "DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	  "DI.CDI_UNIT_COST, B.BCU_PRODUCT_CODE, D.CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,-1) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM BUYER_CATALOGUE_ITEMS B LEFT JOIN " & _
        '	  "CONTRACT_DIST_ITEMS DI ON B.BCU_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	  "AND B.BCU_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX LEFT OUTER JOIN " & _
        '	  "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON B.BCU_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE B.BCU_CAT_INDEX = '" & value1 & "' AND B.BCU_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND CDI_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If
        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND CDI_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "AND BCU_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "UNION " & _
        '	  "SELECT B.BCU_SOURCE, P.PM_VENDOR_ITEM_CODE,B.BCU_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, " & _
        '	  "B.BCU_CAT_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '	  "CONVERT(money,'0.00') as PM_UNIT_COST,B.BCU_PRODUCT_CODE, '' as CDM_END_DATE, " & _
        '	  "ISNULL(BCU_CD_GROUP_INDEX,-1) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM BUYER_CATALOGUE_ITEMS B " & _
        '	  "left JOIN PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE B.BCU_CAT_INDEX = '" & value1 & "' " & _
        '	  "AND B.BCU_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If
        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "AND (BCU_PRODUCT_CODE not IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX  IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "OR " & _
        '	  "BCU_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX not IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'))"

        '	' strgetItem = strgetItem & "))"

        '	dsbuyer = objDb.FillDs(strgetItem)
        '	getBuyerCat_CDP = dsbuyer
        'End Function
        Function getBuyerCatItems(Optional ByVal pCatIndex As String = "", Optional ByVal pItemCode As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal strVendorName As String = "") As Object 'Michelle (25/10/2010)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE " &
                             "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " &
                             "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " &
                             "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " &
                             "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " &
                             "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'  AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex <> "" Then
                strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
            End If
            If pName <> "" Then
                strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
            End If
            If pComType <> "" Then
                strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
            End If
            If pItemCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE ='" & pItemCode & "' "
                'If pItemType.Count > 0 Then
                '    For i As Integer = 0 To pItemType.Count - 1
                '        If strType = "" Then
                '            strType = "'" & pItemType(i) & "'"
                '        Else
                '            strType = strType & "," & "'" & pItemType(i) & "'"
                '        End If
                '    Next
                '    strType = "(" & strType & ")"
                '    If strType <> "" Then
                '        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                '    End If
                'End If
            End If

            If strVendorName <> "" Then
                strSQL &= "AND (CM_P.CM_COY_NAME LIKE '%" & strVendorName & "%' OR CM_1.CM_COY_NAME LIKE '%" & strVendorName & "%' " &
                        "OR CM_2.CM_COY_NAME LIKE '%" & strVendorName & "%' OR CM_3.CM_COY_NAME LIKE '%" & strVendorName & "%') "
            End If

            If pItemType IsNot Nothing Then
                If pItemType.Count > 0 Then
                    For i As Integer = 0 To pItemType.Count - 1
                        If strType = "" Then
                            strType = "'" & pItemType(i) & "'"
                        Else
                            strType = strType & "," & "'" & pItemType(i) & "'"
                        End If
                    Next
                    strType = "(" & strType & ")"
                    If strType <> "" Then
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItems = ds
        End Function
        'add controller 
        Function getBuyerCatItemsByCombo(Optional ByVal CompanyID As String = "", Optional ByVal pCatIndex As String = "", Optional ByVal pItemCode As String = "", Optional ByVal pItemType As ArrayList = Nothing, Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal strVendorName As String = "") As String  'Created by Joon (01 Apr 2011)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE as productcode , PM_VENDOR_ITEM_CODE as vendoritemcode , PM_PRODUCT_DESC as productdesc, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR as currency , PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE as productcode, PM_VENDOR_ITEM_CODE as vendoritemcode, PM_PRODUCT_DESC as productdesc, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR  as currency, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE as txnprice " &
                    "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " &
                    "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " &
                    "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " &
                    "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " &
                    "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " &
                    "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " &
                    "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & CompanyID & "' AND PM_S_COY_ID = '" & CompanyID & "'"

            If pCatIndex = "0" Then
                strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "

            Else
                If pCatIndex <> "" Then
                    strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "
                End If
                If pName <> "" Then
                    strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
                End If
                If pComType <> "" Then
                    strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
                End If
            End If
            If pItemCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE ='" & pItemCode & "' "
                'If pItemType.Count > 0 Then
                '    For i As Integer = 0 To pItemType.Count - 1
                '        If strType = "" Then
                '            strType = "'" & pItemType(i) & "'"
                '        Else
                '            strType = strType & "," & "'" & pItemType(i) & "'"
                '        End If
                '    Next
                '    strType = "(" & strType & ")"
                '    If strType <> "" Then
                '        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                '    End If
                'End If
            End If

            If strVendorName <> "" Then
                strSQL &= "AND (CM_P.CM_COY_NAME LIKE '%" & strVendorName & "%' OR CM_1.CM_COY_NAME LIKE '%" & strVendorName & "%' " &
                        "OR CM_2.CM_COY_NAME LIKE '%" & strVendorName & "%' OR CM_3.CM_COY_NAME LIKE '%" & strVendorName & "%') "
            End If

            If pItemType IsNot Nothing Then
                If pItemType.Count > 0 Then
                    For i As Integer = 0 To pItemType.Count - 1
                        If strType = "" Then
                            strType = "'" & pItemType(i) & "'"
                        Else
                            strType = strType & "," & "'" & pItemType(i) & "'"
                        End If
                    Next
                    strType = "(" & strType & ")"
                    If strType <> "" Then
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
            End If

            'ds = objDb.FillDs(strSQL)
            'getBuyerCatItemsByCombo = ds
            Return strSQL
        End Function
        'add controller 
        Function getBuyerCatItems1(Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal pCode As String = "", Optional ByVal pItemType As ArrayList = Nothing) As Object 'Joon (02/06/2011)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id, (SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id1, CAST(IFNULL(PM_LAST_TXN_TAX, 0) AS UNSIGNED) AS PM_LAST_TXN_TAX " &
                             "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " &
                             "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " &
                             "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " &
                             "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " &
                             "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'  AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' "

            If pCatIndex <> "" Then
                strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
            End If
            If pName <> "" Then
                strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
            End If
            If pCode <> "" Then
                strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(pCode) & "%' "
            End If
            If pComType <> "" Then
                strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
            End If
            If pItemType IsNot Nothing Then
                If pItemType.Count > 0 Then
                    For i As Integer = 0 To pItemType.Count - 1
                        If strType = "" Then
                            strType = "'" & pItemType(i) & "'"
                        Else
                            strType = strType & "," & "'" & pItemType(i) & "'"
                        End If
                    Next
                    strType = "(" & strType & ")"
                    If strType <> "" Then
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
            End If

            ds = objDb.FillDs(strSQL)
            getBuyerCatItems1 = ds
        End Function
        'add controller 
        Function getBuyerCatItemsByCombo1(Optional ByVal CompanyID As String = "", Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False, Optional ByVal pCode As String = "", Optional ByVal pItemType As ArrayList = Nothing) As String 'Joon (02/06/2011)
            Dim strSQL As String
            Dim ds As DataSet
            Dim strType As String

            If pGroup Then 'Select distinct items for Buyer Catalogue Search
                strSQL = "SELECT DISTINCT PM_PRODUCT_CODE as productcode, PM_VENDOR_ITEM_CODE as vendoritemcode, PM_PRODUCT_DESC as productdesc, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR as currency, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            Else
                strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
            End If
            strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE AS txnprice, (SELECT CM_COY_NAME FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id, (SELECT CM_COY_ID FROM COMPANY_MSTR WHERE CM_COY_ID = PRODUCT_MSTR.pm_last_txn_s_coy_id) as pm_last_txn_s_coy_id1, CAST(IFNULL(PM_LAST_TXN_TAX, 0) AS UNSIGNED) AS PM_LAST_TXN_TAX " &
                             "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " &
                             "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " &
                             "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " &
                             "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " &
                             "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " &
                             "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & CompanyID & "' AND PM_S_COY_ID = '" & CompanyID & "' "

            If pCatIndex = "0" Then
                strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "

            Else
                If pCatIndex <> "" Then
                    strSQL &= "AND BCU_CAT_INDEX in (" & pCatIndex & ") "
                End If
                If pName <> "" Then
                    strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
                End If
                If pCode <> "" Then
                    strSQL &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(pCode) & "%' "
                End If
                If pComType <> "" Then
                    strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
                End If
                If pItemType.Count > 0 Then
                    For i As Integer = 0 To pItemType.Count - 1
                        If strType = "" Then
                            strType = "'" & pItemType(i) & "'"
                        Else
                            strType = strType & "," & "'" & pItemType(i) & "'"
                        End If
                    Next
                    strType = "(" & strType & ")"
                    If strType <> "" Then
                        strSQL &= " AND PM_ITEM_TYPE IN " & strType
                    End If
                End If
            End If

            Return strSQL
        End Function

        'Function getBuyerCatItemsFiltered(ByVal sCompanyId As String, ByVal sSelectedProductCode As String, Optional ByVal pCatIndex As String = "", Optional ByVal pComType As String = "", Optional ByVal pName As String = "", Optional ByVal pGroup As Boolean = False) As Object
        '    Dim strSQL As String
        '    Dim ds As DataSet

        '    If pGroup Then 'Select distinct items for Buyer Catalogue Search
        '        strSQL = "SELECT DISTINCT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
        '    Else
        '        strSQL = "SELECT BCI_ITEM_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_LAST_TXN_PRICE_CURR, PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
        '    End If

        '    If sSelectedProductCode <> "" Then
        '        strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE " & _
        '                         "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
        '                         "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
        '                         "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'" & _
        '                         " AND (PM_PREFER_S_COY_ID = '" & sCompanyId & "' OR PM_1ST_S_COY_ID = '" & sCompanyId & "' OR PM_2ND_S_COY_ID = '" & sCompanyId & "' OR  PM_3RD_S_COY_ID= '" & sCompanyId & "')" & _
        '                        " AND NOT PM_PRODUCT_CODE IN (" & sSelectedProductCode & ")"
        '    Else
        '        strSQL &= "CM_P.CM_COY_NAME AS PREFER, CM_1.CM_COY_NAME AS 1ST, CM_2.CM_COY_NAME AS 2ND, CM_3.CM_COY_NAME AS 3RD, PM_LAST_TXN_PRICE " & _
        '                         "FROM BUYER_CATALOGUE_ITEMS INNER JOIN PRODUCT_MSTR ON BCU_PRODUCT_CODE = PM_PRODUCT_CODE " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_P ON PM_PREFER_S_COY_ID = CM_P.CM_COY_ID " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_1 ON PM_1ST_S_COY_ID = CM_1.CM_COY_ID  " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_2 ON PM_2ND_S_COY_ID = CM_2.CM_COY_ID " & _
        '                         "LEFT JOIN COMPANY_MSTR CM_3 ON PM_3RD_S_COY_ID = CM_3.CM_COY_ID " & _
        '                         "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID " & _
        '                         "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_FOR = 'B' AND BCU_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "' AND PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'" & _
        '                         " AND (PM_PREFER_S_COY_ID = '" & sCompanyId & "' OR PM_1ST_S_COY_ID = '" & sCompanyId & "' OR PM_2ND_S_COY_ID = '" & sCompanyId & "' OR  PM_3RD_S_COY_ID= '" & sCompanyId & "')"

        '    End If
        '    If pCatIndex <> "" Then
        '        strSQL &= "AND BCU_CAT_INDEX ='" & pCatIndex & "' "
        '    End If
        '    If pName <> "" Then
        '        strSQL &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(pName) & "%' "
        '    End If
        '    If pComType <> "" Then
        '        strSQL &= "AND PM_CATEGORY_NAME ='" & Common.Parse(pComType) & "' "
        '    End If

        '    ds = objDb.FillDs(strSQL)
        '    getBuyerCatItemsFiltered = ds
        'End Function

        'Date: 7 July 2009
        'Author: Yik Foong
        'Modify from : Function getBuyerCat_CDP( String, String, String )
        'Description:
        '	Added 3 new parameters to support the new search criteria
        'New Search Criteria: Vendor Item Code,Vendor Name and Buyer Item Code at PersonalSetting/Favs_BuyerList.aspx

        'Function getBuyerCat_CDP(ByVal value1 As String, ByVal strcode As String, _
        ' ByVal strVendorItmCode As String, ByVal strBuyerItmCode As String, ByVal vendorName As String, _
        ' ByVal strdesc As String)
        '	Dim strgetItem As String
        '	Dim dsbuyer As DataSet

        '          strgetItem = "SELECT B.BCU_SOURCE,P.PM_VENDOR_ITEM_CODE,PM_S_COY_ID, CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '           "P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, B.BCU_PRODUCT_CODE, DateAdd(Year,1000,getdate()) as CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,0) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '           "FROM BUYER_CATALOGUE_ITEMS B INNER JOIN " & _
        '           "PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' LEFT OUTER JOIN " & _
        '           "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '           "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '           "WHERE B.BCU_CAT_INDEX ='" & value1 & "' AND B.BCU_SOURCE='LP' AND CM.CM_DELETED<>'Y' "             'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If

        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "UNION " & _
        '	  "SELECT B.BCU_SOURCE,DI.CDI_VENDOR_ITEM_CODE,B.BCU_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, B.BCU_CAT_INDEX, " & _
        '	  "DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	  "DI.CDI_UNIT_COST, B.BCU_PRODUCT_CODE, D.CDM_END_DATE,ISNULL(BCU_CD_GROUP_INDEX,-1) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM BUYER_CATALOGUE_ITEMS B LEFT JOIN " & _
        '	  "CONTRACT_DIST_ITEMS DI ON B.BCU_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	  "AND B.BCU_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX LEFT OUTER JOIN " & _
        '	  "COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON B.BCU_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE B.BCU_CAT_INDEX = '" & value1 & "' AND B.BCU_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND CDI_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If

        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND CDI_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "AND BCU_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "UNION " & _
        '	  "SELECT B.BCU_SOURCE, P.PM_VENDOR_ITEM_CODE,B.BCU_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, " & _
        '	  "B.BCU_CAT_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '	  "CONVERT(money,'0.00') as PM_UNIT_COST,B.BCU_PRODUCT_CODE, '' as CDM_END_DATE, " & _
        '	  "ISNULL(BCU_CD_GROUP_INDEX,-1) AS BCU_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM BUYER_CATALOGUE_ITEMS B " & _
        '	  "left JOIN PRODUCT_MSTR P ON B.BCU_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON B.BCU_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE B.BCU_CAT_INDEX = '" & value1 & "' " & _
        '	  "AND B.BCU_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strcode <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strcode)
        '	End If

        '	If strdesc <> "" Then
        '		strgetItem = strgetItem & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If

        '	strgetItem &= "AND (BCU_PRODUCT_CODE not IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX  IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "OR " & _
        '	  "BCU_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND BCU_CD_GROUP_INDEX not IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'))"

        '	dsbuyer = objDb.FillDs(strgetItem)

        '	'Yik Foong, 8 July 2009
        '	'At this point, [dsbuyer] is a dataset from 'previous' version,
        '	'and it does not filter base on the 3 new categories(Buyer Item Code,
        '	'Vendor Item Code, Vendor Name)
        '	'
        '	'Codes belows are starting to filter the result(s) from [dsbuyer] that are 
        '	'match the 3 new categories



        '	'Generating filter expression
        '	'Step 1: Using an array list to store the separate filter condition for
        '	'		each data field. If the input from user is not empty, then the 
        '	'		filter condition is looks like:
        '	'			[ColumnName] LIKE %[userInput]%
        '	'Note: In this condition, it is no need to use Common.ParseSQL(String) Function
        '	'		to process [userInput], it is because system is needed to find the string
        '	'		that are similar to the [userInput]
        '	'[ColumnsName] :
        '	'	1. BCU_SOURCE
        '	'	2. PM_VENDOR_ITEM_CODE
        '	'	3. PM_S_COY_ID
        '	'	4. CM_COY_NAME
        '	'	5. CBC_B_ITEM_CODE
        '	'	6. CBC_B_COY_ID
        '	'	7. BCU_CAT_INDEX
        '	'	8. PM_PRODUCT_DESC
        '	'	9. PM_UOM
        '	'	10. PM_CURRENCY_CODE
        '	'	11. PM_UNIT_COST
        '	'	12. BCU_PRODUCT_CODE
        '	'	13. CDM_END_DATE
        '	'	14. BCU_CD_GROUP_INDEX
        '	'	15. CM_STATUS
        '	'[UserInput]: The search value input by user( pass in from argument from 
        '	'			PersonalSetting/Favs_BuyerList.aspx
        '	'

        '	Dim list As System.Collections.ArrayList = New System.Collections.ArrayList
        '	If strVendorItmCode <> "" Then
        '		list.Add(" PM_VENDOR_ITEM_CODE LIKE '%" & strVendorItmCode & "%'")
        '	End If

        '	If strBuyerItmCode <> "" Then
        '		list.Add(" CBC_B_ITEM_CODE LIKE '%" & strBuyerItmCode & "%'")
        '	End If

        '	If vendorName <> "" Then
        '		list.Add(" CM_COY_NAME LIKE '%" & vendorName & "%'")
        '	End If

        '	'Step 2: iterate the array list and build the filter string
        '	Dim strFilter As String = ""			' filter expression for datatable
        '	For i As Integer = 0 To list.Count - 1
        '		If strFilter <> "" Then
        '			strFilter = strFilter & " AND "
        '		End If
        '		strFilter = strFilter & CStr(list(i))
        '	Next
        '	'Finish generate filter expression

        '	'Clone the original dataset, but does not clone the data
        '	Dim ds As DataSet = dsbuyer.Clone
        '	Dim rows As DataRow()
        '	'Select the row(s) that match the filter condition
        '	'Note: Further filter features should process at here( modify the [strFilter] )
        '	rows = dsbuyer.Tables(0).Select(strFilter)

        '	'Inserting selected row(s) into another cloned dataset's table
        '	'Note: Don't clear the rows from the source datatable and copy the 
        '	'		selected row into the source table again. This is because
        '	'		when clearing the row(s) from the source table, the "selected" rows
        '	'		will also deleted, thus it will fail to insert the selected rows
        '	'		into another table
        '	For Each row As DataRow In rows
        '		ds.Tables(0).ImportRow(row)
        '	Next

        '	'getBuyerCat_CDP = dsbuyer => original version return [dsbuyer]
        '	getBuyerCat_CDP = ds

        '	'Finish implementing new search criteria
        'End Function


        'Function getFavCat_CDP(ByVal value2 As String, Optional ByVal strdesc As String = "")
        '	Dim strgetfav As String
        '	Dim dsfav As DataSet

        '	'strgetfav = "SELECT F.FLI_SOURCE, P.PM_VENDOR_ITEM_CODE, PM_S_COY_ID,CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, F.FLI_LIST_INDEX, " & _
        '	'            "P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, P.PM_UNIT_COST, F.FLI_PRODUCT_CODE, DateAdd(Year, 1000, getdate()) as CDM_END_DATE,ISNULL(FLI_CD_GROUP_INDEX,0) AS FLI_CD_GROUP_INDEX " & _
        '	'            "FROM FAVOURITE_LIST_ITEMS F INNER JOIN " & _
        '	'            "PRODUCT_MSTR P ON F.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE LEFT OUTER JOIN " & _
        '	'            "COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	'            "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	'            "where F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE='LP' " & _
        '	'            "UNION " & _
        '	'            "SELECT F.FLI_SOURCE, DI.CDI_VENDOR_ITEM_CODE, F.FLI_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, F.FLI_LIST_INDEX, " & _
        '	'            "DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	'            "DI.CDI_UNIT_COST, F.FLI_PRODUCT_CODE, D.CDM_END_DATE,ISNULL(FLI_CD_GROUP_INDEX,-1) AS FLI_CD_GROUP_INDEX " & _
        '	'            "FROM FAVOURITE_LIST_ITEMS F LEFT JOIN " & _
        '	'            "CONTRACT_DIST_ITEMS DI ON F.FLI_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	'            "AND F.FLI_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	'            "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX LEFT OUTER JOIN " & _
        '	'            "COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE AND CBC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " & _
        '	'            "INNER JOIN COMPANY_MSTR CM ON F.FLI_S_COY_ID = CM.CM_COY_ID " & _
        '	'            "WHERE F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE IN ('CP','DP')"

        '	' ai chu modified on 09/12/2005
        '	' deactivated company's item will also be displayed but with different color
        '          strgetfav = "SELECT F.FLI_SOURCE, P.PM_VENDOR_ITEM_CODE, PM_S_COY_ID,CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, " & _
        '             "C.CBC_B_COY_ID, F.FLI_LIST_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '             "P.PM_UNIT_COST, F.FLI_PRODUCT_CODE, DateAdd(Year,1000,getdate()) as CDM_END_DATE, " & _
        '             "ISNULL(FLI_CD_GROUP_INDEX,0) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS FROM FAVOURITE_LIST_ITEMS F " & _
        '             "INNER JOIN PRODUCT_MSTR P ON F.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '             "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '             "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '             "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '             "where F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE='LP' AND CM.CM_DELETED<>'Y' "             'AND CM.CM_STATUS='A' "
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "UNION " & _
        '	  "SELECT F.FLI_SOURCE, DI.CDI_VENDOR_ITEM_CODE, F.FLI_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, " & _
        '	  "C.CBC_B_COY_ID, F.FLI_LIST_INDEX, DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	  "DI.CDI_UNIT_COST, F.FLI_PRODUCT_CODE, D.CDM_END_DATE, " & _
        '	  "ISNULL(FLI_CD_GROUP_INDEX,-1) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM FAVOURITE_LIST_ITEMS F " & _
        '	  "LEFT JOIN CONTRACT_DIST_ITEMS DI ON F.FLI_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	  "AND F.FLI_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON F.FLI_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND DI.CDI_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "AND FLI_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "union " & _
        '	  "SELECT F.FLI_SOURCE, P.PM_VENDOR_ITEM_CODE, F.FLI_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, " & _
        '	  "F.FLI_LIST_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '	  "CONVERT(money,'0.00') as PM_UNIT_COST, F.FLI_PRODUCT_CODE, '' as CDM_END_DATE, " & _
        '	  "ISNULL(FLI_CD_GROUP_INDEX,-1) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM FAVOURITE_LIST_ITEMS F " & _
        '	  "left JOIN PRODUCT_MSTR P ON F.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE F.FLI_LIST_INDEX = '" & value2 & "' " & _
        '	  "AND CM.CM_DELETED<>'Y'  " & _
        '	  "AND F.FLI_SOURCE IN ('CP','DP') "
        '	'AND CM.CM_STATUS='A'
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "AND (FLI_PRODUCT_CODE not IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX  IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "OR " & _
        '	  "FLI_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX not IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'))"
        '	dsfav = objDb.FillDs(strgetfav)
        '	getFavCat_CDP = dsfav
        'End Function


        'Date: 7 July 2009
        'Author: Yik Foong
        'Modify from : Function getBuyerCat_CDP( String, String, String )
        'Description:
        '	Added 3 new parameters to support the new search criteria
        'New Search Criteria: Vendor Item Code,Vendor Name and Buyer Item Code at PersonalSetting/Favs_BuyerList.aspx
        'Function getFavCat_CDP(ByVal value2 As String, _
        ' ByVal strVendorItmCode As String, ByVal strBuyerItmCode As String, ByVal vendorName As String, _
        ' Optional ByVal strdesc As String = "")
        '	Dim strgetfav As String
        '	Dim dsfav As DataSet


        '          strgetfav = "SELECT F.FLI_SOURCE, P.PM_VENDOR_ITEM_CODE, PM_S_COY_ID,CM.CM_COY_NAME, C.CBC_B_ITEM_CODE, " & _
        '             "C.CBC_B_COY_ID, F.FLI_LIST_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '             "P.PM_UNIT_COST, F.FLI_PRODUCT_CODE, DateAdd(Year,1000,getdate()) as CDM_END_DATE, " & _
        '             "ISNULL(FLI_CD_GROUP_INDEX,0) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS FROM FAVOURITE_LIST_ITEMS F " & _
        '             "INNER JOIN PRODUCT_MSTR P ON F.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '             "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '             "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '             "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '             "where F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE='LP' AND CM.CM_DELETED<>'Y' "             'AND CM.CM_STATUS='A' "
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "UNION " & _
        '	  "SELECT F.FLI_SOURCE, DI.CDI_VENDOR_ITEM_CODE, F.FLI_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, " & _
        '	  "C.CBC_B_COY_ID, F.FLI_LIST_INDEX, DI.CDI_PRODUCT_DESC, DI.CDI_UOM, DI.CDI_CURRENCY_CODE, " & _
        '	  "DI.CDI_UNIT_COST, F.FLI_PRODUCT_CODE, D.CDM_END_DATE, " & _
        '	  "ISNULL(FLI_CD_GROUP_INDEX,-1) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM FAVOURITE_LIST_ITEMS F " & _
        '	  "LEFT JOIN CONTRACT_DIST_ITEMS DI ON F.FLI_PRODUCT_CODE = DI.CDI_PRODUCT_CODE " & _
        '	  "AND F.FLI_CD_GROUP_INDEX = DI.CDI_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR D ON DI.CDI_GROUP_INDEX = D.CDM_GROUP_INDEX " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON F.FLI_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE F.FLI_LIST_INDEX = '" & value2 & "' AND F.FLI_SOURCE IN ('CP','DP') AND CM.CM_DELETED<>'Y' "			 'AND CM.CM_STATUS='A' "
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND DI.CDI_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "AND FLI_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "union " & _
        '	  "SELECT F.FLI_SOURCE, P.PM_VENDOR_ITEM_CODE, F.FLI_S_COY_ID ,CM_COY_NAME, C.CBC_B_ITEM_CODE, C.CBC_B_COY_ID, " & _
        '	  "F.FLI_LIST_INDEX, P.PM_PRODUCT_DESC, P.PM_UOM, P.PM_CURRENCY_CODE, " & _
        '	  "CONVERT(money,'0.00') as PM_UNIT_COST, F.FLI_PRODUCT_CODE, '' as CDM_END_DATE, " & _
        '	  "ISNULL(FLI_CD_GROUP_INDEX,-1) AS FLI_CD_GROUP_INDEX, CM.CM_STATUS " & _
        '	  "FROM FAVOURITE_LIST_ITEMS F " & _
        '	  "left JOIN PRODUCT_MSTR P ON F.FLI_PRODUCT_CODE = P.PM_PRODUCT_CODE AND P.PM_DELETED <> 'Y' " & _
        '	  "LEFT OUTER JOIN COMPANY_B_ITEM_CODE C ON F.FLI_PRODUCT_CODE = C.CBC_PRODUCT_CODE " & _
        '	  "AND CBC_B_COY_ID='" & HttpContext.Current.Session("CompanyID") & "' " & _
        '	  "INNER JOIN COMPANY_MSTR CM ON P.PM_S_COY_ID = CM.CM_COY_ID " & _
        '	  "WHERE F.FLI_LIST_INDEX = '" & value2 & "' " & _
        '	  "AND CM.CM_DELETED<>'Y'  " & _
        '	  "AND F.FLI_SOURCE IN ('CP','DP') "
        '	'AND CM.CM_STATUS='A'
        '	If strdesc <> "" Then
        '		strgetfav = strgetfav & "AND P.PM_PRODUCT_DESC" & Common.ParseSQL(strdesc)
        '	End If
        '	strgetfav &= "AND (FLI_PRODUCT_CODE not IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX  IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "OR " & _
        '	  "FLI_PRODUCT_CODE IN " & _
        '	  "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "') " & _
        '	  "AND FLI_CD_GROUP_INDEX not IN " & _
        '	  "(SELECT CDI_GROUP_INDEX FROM CONTRACT_DIST_ITEMS " & _
        '	  "LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND (CDM_TYPE = 'C' or CDM_TYPE = 'D') " & _
        '	  "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'))"
        '	dsfav = objDb.FillDs(strgetfav)



        '	'Yik Foong, 8 July 2009
        '	'At this point, [dsbuyer] is a dataset from 'previous' version,
        '	'and it does not filter base on the 3 new categories(Buyer Item Code,
        '	'Vendor Item Code, Vendor Name)
        '	'
        '	'Codes belows are starting to filter the result(s) from [dsbuyer] that are 
        '	'match the 3 new categories



        '	'Generating filter expression
        '	'Step 1: Using an array list to store the separate filter condition for
        '	'		each data field. If the input from user is not empty, then the 
        '	'		filter condition is looks like:
        '	'			[ColumnName] LIKE %[userInput]%
        '	'Note: In this condition, it is no need to use Common.ParseSQL(String) Function
        '	'		to process [userInput], it is because system is needed to find the string
        '	'		that are similar to the [userInput]
        '	'[ColumnsName] :
        '	'	1. FLI_SOURCE
        '	'	2. PM_VENDOR_ITEM_CODE
        '	'	3. PM_S_COY_ID
        '	'	4. CM_COY_NAME
        '	'	5. CBC_B_ITEM_CODE
        '	'	6. CBC_B_COY_ID
        '	'	7. FLI_LIST_INDEX
        '	'	8. PM_PRODUCT_DESC
        '	'	9. PM_UOM
        '	'	10. PM_CURRENCY_CODE
        '	'	11. PM_UNIT_COST
        '	'	12. FLI_PRODUCT_CODE
        '	'	13. CDM_END_DATE
        '	'	14. FLI_CD_GROUP_INDEX
        '	'	15. CM_STATUS
        '	'[UserInput]: The search value input by user( pass in from argument from 
        '	'			PersonalSetting/Favs_BuyerList.aspx
        '	'

        '	Dim list As System.Collections.ArrayList = New System.Collections.ArrayList
        '	If strVendorItmCode <> "" Then
        '		list.Add(" PM_VENDOR_ITEM_CODE LIKE '%" & strVendorItmCode & "%'")
        '	End If

        '	If strBuyerItmCode <> "" Then
        '		list.Add(" CBC_B_ITEM_CODE LIKE '%" & strBuyerItmCode & "%'")
        '	End If

        '	If vendorName <> "" Then
        '		list.Add(" CM_COY_NAME LIKE '%" & vendorName & "%'")
        '	End If

        '	'Step 2: iterate the array list and build the filter string
        '	Dim strFilter As String = ""			' filter expression for datatable
        '	For i As Integer = 0 To list.Count - 1
        '		If strFilter <> "" Then
        '			strFilter = strFilter & " AND "
        '		End If
        '		strFilter = strFilter & CStr(list(i))
        '	Next
        '	'Finish generate filter expression

        '	'Clone the original dataset, but does not clone the data
        '	Dim ds As DataSet = dsfav.Clone
        '	Dim rows As DataRow()
        '	'Select the row(s) that match the filter condition
        '	'Note: Further filter features should process at here( modify the [strFilter] )
        '	rows = dsfav.Tables(0).Select(strFilter)

        '	'Inserting selected row(s) into another cloned dataset's table
        '	'Note: Don't clear the rows from the source datatable and copy the 
        '	'		selected row into the source table again. This is because
        '	'		when clearing the row(s) from the source table, the "selected" rows
        '	'		will also deleted, thus it will fail to insert the selected rows
        '	'		into another table
        '	For Each row As DataRow In rows
        '		ds.Tables(0).ImportRow(row)
        '	Next

        '	getFavCat_CDP = ds
        'End Function


        Public Function bindItem() As DataView

            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strSQL As String = "SELECT BCM_GRP_CODE,BCM_CAT_INDEX, " & objDB.Concat(" : ", "", "BCM_GRP_CODE", "BCM_GRP_DESC") & " as name " &
                        "FROM BUYER_CATALOGUE_MSTR Where BCM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"
            '"AND BCM_STATUS <> 'Y'"
            drw = objDB.GetView(strSQL)
            Return drw

        End Function
        '//By Moo
        Public Function getBuyerCatByUser(ByVal userID As String) As DataView
            'Modified by Joon on 22 Sept 2010
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim objUsers As New Users
            Dim companyID As String = objUsers.getCompanyID(userID)
            'Michelle (3/11/2010) - Remove the Buyer Catalogue code
            'Dim strSQL As String = "SELECT BCM_GRP_CODE,BCM_CAT_INDEX, " & objDB.Concat(" : ", "", "BCM_GRP_CODE", "BCM_GRP_DESC") & " as name " & _
            'Michelle (23/12/2010) - Default Purchaser Catalogue as the 1st in the list
            Dim strSQL As String
            strSQL = "SELECT BCM_CAT_INDEX, BCM_GRP_DESC, '1' AS SEQ " &
            "FROM BUYER_CATALOGUE_MSTR a, BUYER_CATALOGUE_USER b Where " &
            "a.BCM_CAT_INDEX=b.BCU_CAT_INDEX AND BCM_B_COY_ID = '" & companyID & "' " &
            "AND BCU_USER_ID='" & userID & "' AND BCM_GRP_DESC = 'Default Purchaser Catalogue' UNION "
            strSQL &= "SELECT BCM_CAT_INDEX, BCM_GRP_DESC, '2' AS SEQ " &
              "FROM BUYER_CATALOGUE_MSTR a, BUYER_CATALOGUE_USER b Where " &
              "a.BCM_CAT_INDEX=b.BCU_CAT_INDEX AND BCM_B_COY_ID = '" & companyID & "' " &
              "AND BCU_USER_ID='" & userID & "' AND BCM_GRP_DESC <> 'Default Purchaser Catalogue' " &
                "ORDER BY SEQ, BCM_GRP_DESC"
            drw = objDB.GetView(strSQL)
            Return drw
        End Function



        'Michelle (27/10/2010) - To get the Items from Product Mstr
        'Public Function getItems(ByVal strCoyId As String, ByVal strItemType As String, Optional ByVal strBCItemIdx As String = "", Optional ByVal strCode As String = "", Optional ByVal strName As String = "", Optional ByVal strComType As String = "", Optional ByVal strDel As String = "", Optional ByVal pItemType As ArrayList = Nothing) As String
        Public Function getItems(ByVal strCoyId As String, ByVal strItemType As String, Optional ByVal strBCItemIdx As String = "", Optional ByVal strCode As String = "", Optional ByVal strName As String = "", Optional ByVal strComType As String = "", Optional ByVal strDel As String = "", Optional ByVal pItemType As ArrayList = Nothing) As String
            Dim strsql As String
            Dim ds As New DataSet
            'Dim ds As String
            Dim strType As String

            strsql = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, CT_NAME, PM_DELETED "
            strsql &= "FROM PRODUCT_MSTR "
            strsql &= "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME "
            strsql &= "WHERE PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
            strsql &= "AND PM_PRODUCT_FOR = '" & Common.Parse(strItemType) & "' "

            If strDel = "A" Then
                strsql = strsql 'ie no need to check for the indicator
            Else
                strsql &= "AND PM_DELETED = '" & Common.Parse(strDel) & "' "
            End If

            If strCode <> "" Then
                strsql &= "AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strCode) & "%' "
            End If
            If strName <> "" Then
                strsql &= "AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(strName) & "%' "
            End If

            If strComType <> "" Then
                strsql &= "AND PM_CATEGORY_NAME ='" & Common.Parse(strComType) & "' "
            End If

            If strBCItemIdx <> "" Then
                strsql &= "AND PM_PRODUCT_CODE NOT IN "
                strsql &= "(SELECT BCU_PRODUCT_CODE FROM BUYER_CATALOGUE_ITEMS "
                strsql &= "WHERE BCU_CAT_INDEX = '" & Common.Parse(strBCItemIdx) & "') "
            End If

            If pItemType.Count > 0 Then
                For i As Integer = 0 To pItemType.Count - 1
                    If strType = "" Then
                        strType = "'" & pItemType(i) & "'"
                    Else
                        strType = strType & "," & "'" & pItemType(i) & "'"
                    End If
                Next
                strType = "(" & strType & ")"
                If strType <> "" Then
                    strsql &= " AND PM_ITEM_TYPE IN " & strType
                End If
            End If

            'ds = objDb.FillDs(strsql)
            getItems = strsql
        End Function
        Public Function getBuyerCat() As DataView 'Michelle (22/10/2010) - 
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strSQL As String = "Select M.BCM_CAT_INDEX,M.BCM_GRP_DESC as name, 1 AS SEQ " &
                     "FROM BUYER_CATALOGUE_MSTR M " &
                     "where M.BCM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' and M.BCM_GRP_DESC = 'Default Purchaser Catalogue' " &
                     "UNION " &
                     "Select M.BCM_CAT_INDEX,M.BCM_GRP_DESC as name, 2 AS SEQ " &
                     "FROM BUYER_CATALOGUE_MSTR M " &
                     "where M.BCM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' and M.BCM_GRP_DESC <> 'Default Purchaser Catalogue' ORDER BY SEQ, name"
            drw = objDB.GetView(strSQL)
            Return drw
        End Function

        Public Function getBuyerCat1() As DataView 'Michelle (22/10/2010) - 
            Dim drw As DataView
            Dim objDB As New EAD.DBCom
            Dim strSQL As String = "Select M.BCM_CAT_INDEX,M.BCM_GRP_DESC as name, 2 AS SEQ " &
                     "FROM BUYER_CATALOGUE_MSTR M " &
                     "where M.BCM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "' and M.BCM_GRP_DESC <> 'Default Purchaser Catalogue' ORDER BY SEQ, name"
            drw = objDB.GetView(strSQL)
            Return drw
        End Function

        'Function bindselectItem(ByVal value As String)
        '    Dim ds1 As DataSet
        '    Dim strQueryselect As String

        '    strQueryselect = "SELECT U.UM_DEPT_ID,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '                     "(U.UM_DEPT_ID + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '                     "FROM BUYER_CATALOGUE_USER K, USERS_MSTR U " & _
        '                     "where k.BCU_CAT_INDEX = '" & value & "' AND K.BCU_USER_ID = U.UM_USER_ID AND U.UM_COMPANY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "'"


        '    ds1 = objDb.FillDs(strQueryselect)
        '    bindselectItem = ds1

        'End Function

        'Function bindsearchItem(ByVal value As String)
        '    Dim strQuery, strQueryselect As String
        '    Dim ds2 As DataSet
        '    strQuery = "SELECT DISTINCT U.UM_DEPT_ID,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '               "(U.UM_DEPT_ID + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '               "FROM USER_MSTR U, USERS_ROLE R " & _
        '               "where UM_USER_ID NOT IN (SELECT K.BCU_USER_ID from BUYER_CATALOGUE_MSTR B ,BUYER_CATALOGUE_USER K where B.BCM_CAT_INDEX=k.BCU_CAT_INDEX AND B.BCM_CAT_INDEX = '" & value & "' ) " & _
        '                "AND R.UU_USER_ID = U.UM_USER_ID " & _
        '                "AND R.UR_JOBTITLE_ID='Buyer' AND UM_COMPANY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "'"

        '    ds2 = objDb.FillDs(strQuery)

        'End Function
        'Public Function Favlist_bindItem() As DataView

        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    strSql = "SELECT BCM_GRP_CODE,BCM_CAT_INDEX, BCM_GRP_CODE  + ' : ' + BCM_GRP_DESC as name " & _
        '                           "FROM BUYER_CATALOGUE_MSTR " & _
        '                           "WHERE BCM_B_COY_ID ='" & HttpContext.Current.Session("CompanyID") & "'"

        '    drw = objDB.GetView(strSql)
        '    Return drw

        'End Function

        Public Function Favlist_bindFav() As DataView

            Dim strSql As String
            Dim drw As DataView
            Dim objDB As New EAD.DBCom

            strSql = "SELECT FLM_LIST_INDEX,FLM_LIST_NAME as name from FAVOURITE_LIST_MSTR " &
               "WHERE FLM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' " &
               "AND FLM_B_COY_ID = '" & HttpContext.Current.Session("CompanyID") & "'"

            drw = objDB.GetView(strSql)
            Return drw
        End Function

        ' ai chu add on 31/10/2005
        ' to retrieve contract item to be added/deleted into/from buyer catalogue
        ' strMode - A = Add (retrieve from CONTRACT_DIST_ITEMS), D = Delete (retrieve from BUYER_CATALOGUE_ITEMS)
        Public Function getContractItemForBuyerCatalogue(ByVal lngGrpIndex As Long, ByVal strMode As String) As DataTable
            Dim strsql As String
            Dim ds As DataSet
            Select Case UCase(strMode)
                Case "A"
                    strsql = "SELECT CDI_PRODUCT_CODE AS Product_Code, CDI_VENDOR_ITEM_CODE as Vendor_Item_Code, CDI_PRODUCT_DESC, CDI_UOM, "
                    strsql &= "CDI_UNIT_COST as SC_Unit_Cost, CDI_CURRENCY_CODE, "
                    strsql &= "CDM_S_COY_ID as S_Coy_Id, 'CP' as Cat_Type, '' as msg, CDM_GROUP_INDEX as Grp_Index "
                    strsql &= "FROM CONTRACT_DIST_MSTR "
                    strsql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDM_GROUP_INDEX = CDI_GROUP_INDEX "
                    strsql &= "WHERE CDI_GROUP_INDEX = " & lngGrpIndex & " "
                    strsql &= "AND (GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE) "
                Case "D"
                    strsql = "SELECT BCU_PRODUCT_CODE AS Product_Code, CDI_VENDOR_ITEM_CODE as Vendor_Item_Code, CDI_PRODUCT_DESC, CDI_UOM,  "
                    strsql &= "CDI_UNIT_COST as SC_Unit_Cost, CDI_CURRENCY_CODE, "
                    strsql &= "BCU_S_COY_ID as S_Coy_Id, 'CP' as Cat_Type, '' as msg, BCU_CD_GROUP_INDEX as Grp_Index "
                    strsql &= "FROM BUYER_CATALOGUE_ITEMS "
                    strsql &= "INNER JOIN CONTRACT_DIST_ITEMS ON BCU_PRODUCT_CODE = CDI_PRODUCT_CODE AND BCU_CD_GROUP_INDEX = CDI_GROUP_INDEX "
                    strsql &= "WHERE BCU_CD_GROUP_INDEX = " & lngGrpIndex & " AND BCU_SOURCE = 'CP' "
            End Select

            ds = objDb.FillDs(strsql)
            getContractItemForBuyerCatalogue = ds.Tables(0)
        End Function
        'Public Function getItemCode() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT PM_PRODUCT_INDEX,PM_VENDOR_ITEM_CODE " _
        '            & "FROM PRODUCT_MSTR " _
        '            & "WHERE PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
        '    ds = objDb.FillDs(strsql)
        '    getItemCode = ds
        'End Function

        'Public Function getItemName() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT PM_PRODUCT_INDEX,PM_PRODUCT_DESC " _
        '            & "FROM PRODUCT_MSTR " _
        '            & "WHERE PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
        '    ds = objDb.FillDs(strsql)
        '    getItemName = ds
        'End Function

        '// for testing purpose
        Public Function getsubcat(ByVal id As String) As DataSet
            Dim strsql As String
            Dim ds As DataSet

            strsql = "SELECT * FROM LOCATION_MSTR WHERE LM_LOCATION='" & id & "'"
            ds = objDb.FillDs(strsql)
            Return ds
        End Function

        ' ai chu add on 08/11/2005
        ' to delete all/selected contract item(s) from Buyer Catalogue 
    End Class

End Namespace
