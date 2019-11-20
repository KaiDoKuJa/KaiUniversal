namespace Kai.Universal.Sql.Where {

    public enum CriteriaType {
        Direct = 1,

        IsNull = 11,
        IsNotNull = 12,

        Equal = 21,
        NotEqual = 22,
        LessThan = 23,
        LessThanEqual = 24,
        GreaterThan = 25,
        GreaterThanEqual = 26,

        Like = 31,
        NotLike = 32,
        LeftLike = 33,
        RightLike = 34,
        NotLeftLike = 35,
        NotRightLike = 36,

        In = 41,
    }
}