namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// the criteria type
    /// <para></para>
    /// </summary>
    public enum CriteriaType {
        /// <summary>
        /// direct condition
        /// </summary>
        [CriteriaReflect(typeof(DirectCriteria))]
        Direct = 1,

        /// <summary>
        /// is null
        /// </summary>
        [CriteriaReflect(typeof(NullCriteria), " is null")]
        IsNull = 11,
        /// <summary>
        /// is not null
        /// </summary>
        [CriteriaReflect(typeof(NullCriteria), " is not null")]
        IsNotNull = 12,

        /// <summary>
        /// {0} = {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " = ")]
        Equal = 21,
        /// <summary>
        /// {0} &lt;&gt; {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " <> ")]
        NotEqual = 22,
        /// <summary>
        /// {0} &lt; {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " < ")]
        LessThan = 23,
        /// <summary>
        /// {0} &lt;= {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " <= ")]
        LessThanEqual = 24,
        /// <summary>
        /// {0} &gt; {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " > ")]
        GreaterThan = 25,
        /// <summary>
        /// {0} &gt;= {1}
        /// </summary>
        [CriteriaReflect(typeof(CompareCriteria), " >= ")]
        GreaterThanEqual = 26,

        /// <summary>
        /// {0} like '%{1}%'
        /// </summary>
        [CriteriaReflect(typeof(LikeCriteria), " like ")]
        Like = 31,
        /// <summary>
        /// {0} not like '%{1}%'
        /// </summary>
        [CriteriaReflect(typeof(LikeCriteria), " not like ")]
        NotLike = 32,

        /// <summary>
        /// {0} like '%{1}'
        /// </summary>
        [CriteriaReflect(typeof(LeftLikeCriteria), " like ")]
        LeftLike = 33,
        /// <summary>
        /// {0} like '{1}%'
        /// </summary>
        [CriteriaReflect(typeof(RightLikeCriteria), " like ")]
        RightLike = 34,

        /// <summary>
        /// {0} not like '%{1}'
        /// </summary>
        [CriteriaReflect(typeof(LeftLikeCriteria), " not like ")]
        NotLeftLike = 35,
        /// <summary>
        /// {0} not like '{1}%'
        /// </summary>
        [CriteriaReflect(typeof(RightLikeCriteria), " not like ")]
        NotRightLike = 36,

        /// <summary>
        /// {0} in ({1},{2},{3},...)
        /// </summary>
        [CriteriaReflect(typeof(InCriteria), " in ")]
        In = 41,
    }

}