namespace Kai.Universal.Sql.Where {

    public enum CriteriaType {
        [CriteriaReflect(typeof(DirectCriteria))]
        Direct = 1,

        [CriteriaReflect(typeof(NullCriteria), " is null")]
        IsNull = 11,
        [CriteriaReflect(typeof(NullCriteria), " is not null")]
        IsNotNull = 12,

        [CriteriaReflect(typeof(CompareCriteria), " = ")]
        Equal = 21,
        [CriteriaReflect(typeof(CompareCriteria), " <> ")]
        NotEqual = 22,
        [CriteriaReflect(typeof(CompareCriteria), " < ")]
        LessThan = 23,
        [CriteriaReflect(typeof(CompareCriteria), " <= ")]
        LessThanEqual = 24,
        [CriteriaReflect(typeof(CompareCriteria), " > ")]
        GreaterThan = 25,
        [CriteriaReflect(typeof(CompareCriteria), " >= ")]
        GreaterThanEqual = 26,

        [CriteriaReflect(typeof(LikeCriteria), " like ")]
        Like = 31,
        [CriteriaReflect(typeof(LikeCriteria), " not like ")]
        NotLike = 32,

        [CriteriaReflect(typeof(LeftLikeCriteria), " like ")]
        LeftLike = 33,
        [CriteriaReflect(typeof(RightLikeCriteria), " like ")]
        RightLike = 34,

        [CriteriaReflect(typeof(LeftLikeCriteria), " not like ")]
        NotLeftLike = 35,
        [CriteriaReflect(typeof(RightLikeCriteria), " not like ")]
        NotRightLike = 36,

        [CriteriaReflect(typeof(InCriteria), " in ")]
        In = 41,
    }

}