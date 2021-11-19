<?php
#
# ВНИМАНИЕ! Это временный файл, его редактирование бессмысленно!
#
?>
<?php
$cfg['Servers'][1]['verbose']         = '';
$cfg['Servers'][1]['host']            = '127.0.0.1';
$cfg['Servers'][1]['port']            = 3306;
$cfg['Servers'][1]['socket']          = '';
$cfg['Servers'][1]['connect_type']    = 'tcp';
$cfg['Servers'][1]['compress']        = false;
$cfg['Servers'][1]['extension']       = 'mysqli';
$cfg['Servers'][1]['auth_type']       = 'cookie';
$cfg['Servers'][1]['AllowRoot']       = true;
$cfg['Servers'][1]['nopassword']      = true;
$cfg['Servers'][1]['AllowNoPassword'] = true;
$cfg['ActionLinksMode']               = 'icons';
$cfg['AjaxEnable']                    = true;
$cfg['blowfish_secret']               = 'qfAUHkdeNQ94aaB4YhMxRNvRWYVFax64';
$cfg['MaxRows']                       = 50;
$cfg['ServerDefault']                 = 1;
$cfg['ShowAll']                       = true;
$cfg['SaveDir']                       = 'c:/openserver/userdata/temp/upload';
$cfg['UploadDir']                     = 'c:/openserver/userdata/temp/upload';
$cfg['VersionCheck']                  = false;
$cfg['TabsMode']                      = 'both';
$cfg['TableNavigationLinksMode']      = 'icons';
$cfg['ThemeDefault']                  = 'darkwolf';
//$cfg['ThemeManager'] = false;

/**
* disallow editing of binary fields
* valid values are:
*   false    allow editing
*   'blob'   allow editing except for BLOB fields
*   'noblob' disallow editing except for BLOB fields
*   'all'    disallow editing
* default = blob
*/
//$cfg['ProtectBinary'] = 'false';

$cfg['DefaultLang']                            = 'ru';
$cfg['ServerLibraryDifference_DisableWarning'] = true;
$cfg['PmaNoRelation_DisableWarning']           = true;

/**
* default display direction (horizontal|vertical|horizontalflipped)
*/
$cfg['DefaultDisplay'] = 'horizontal';

/**
* How many columns should be used for table display of a database?
* (a value larger than 1 results in some information being hidden)
* default = 1
*/
//$cfg['PropertiesNumColumns'] = 2;

/**
* Whether to display icons or text or both icons and text in table row
* action segment. Value can be either of 'icons', 'text' or 'both'.
* default = 'both'
*/
//$cfg['RowActionType'] = 'icons';

/**
* How many columns should be used for table display of a database?
* (a value larger than 1 results in some information being hidden)
* default = 1
*/
//$cfg['PropertiesNumColumns'] = 2;

/**
* Set to true if you want DB-based query history.If false, this utilizes
* JS-routines to display query history (lost by window close)
*
* This requires configuration storage enabled, see above.
* default = false
*/
//$cfg['QueryHistoryDB'] = true;

/**
* When using DB-based query history, how many entries should be kept?
* default = 25
*/
//$cfg['QueryHistoryMax'] = 100;

/**
* Whether or not to query the user before sending the error report to
* the phpMyAdmin team when a JavaScript error occurs
*
* Available options
* ('ask' | 'always' | 'never')
* default = 'ask'
*/
//$cfg['SendErrorReports'] = 'always';
