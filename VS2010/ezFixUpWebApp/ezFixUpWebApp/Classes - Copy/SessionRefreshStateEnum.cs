using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ezFixUp.Data
{
    public enum SessionRefreshStateEnum
    {
        AddFriendsWithMutualFriends = 0,
        CacheSmallImagesOfFriendsWithMutualFriends = 1,
        CacheNormalImagesOfFriendsWithMutualFriends = 2,
        LoadPhotosOfFriendsWithMutualFriends = 3,
        AddFriendsWithoutMutualFriends = 4,
        LoadPhotosOfFriendsWithoutMutualFriends = 5,
        Idle = 6
    }
}
