Imports Microsoft.VisualBasic

Public Enum enumMultiTypes
  Parent = 1
  Child = 2
  Linked = 3
  Forward = 4
  Backward = 5
  Associated = 6
  Authorized = 7
  Searched = 8
  AllowedTypes = 9
  Administrator = 10
  Created = 11
End Enum
Public Enum enumDMSStates
  Created = 1
  CheckedIn = 2
  CheckedOut = 3
  UnderVerification = 4
  UnderApproval = 5
  Published = 6
  UnderRevision = 7
  Superseded = 8
  Expired = 9
  Closed = 10
  Approved = 11
  Rejected = 12
  DMSError = 13
End Enum
Public Enum enumQualities
  NoRestriction = 1
  NotAllowed = 2
  CreatorOnly = 3
  AllAuthorized = 4
  AllAuthorizedSameCompany = 5
  AllAuthorizedSameDivision = 6
  AllAuthorizedSameDepartment = 7
  AllAuthorizedSameProject = 8
  AllAuthorizedSameWBS = 9
  HavingKeyword = 10
  UseWorkflow = 11
  AllItems = 12
  AuthorizedItems = 13
End Enum
Public Enum enumItemTypes
  Folder = 1
  FolderGroup = 2
  File = 3
  FileGroup = 4
  User = 5
  UserGroup = 6
  Tag = 7
  Link = 8
  UValue = 9
  Workflow = 10
  Authorization = 11
  GrantedToMe = 12
  UnderApprovalToMe = 13
  ApprovedByMe = 14
  Searches = 15
  Projects = 16
  UserGroupUser = 17
  Selected = 18
  Fevorites = 19
  SelectedFile = 20
End Enum
