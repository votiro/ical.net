namespace Experiments.Utilities
{
    /// <summary>
    ///
    /// https://tools.ietf.org/html/rfc5545#section-2.1
    /// </summary>
    public static class FormattingCodepoints
    {
        /// <summary>
        /// Horizontal tab (HTAB)
        /// </summary>
        public const string Tab = "\t";

        /// <summary>
        /// Line feed (LF)
        /// </summary>
        public const string Lf = "\n";

        /// <summary>
        /// Carriage return (CR)
        /// </summary>
        public const string Cr = "\r";

        /// <summary>
        /// Double quote (DQUOTE)
        /// </summary>
        public const string Dquote = "\"";

        //... Etc.

        /// <summary>
        /// Forward-slash (SOLIDUS)
        /// </summary>
        public const string Solidus = "/";

        /// <summary>
        /// The line break mandated by the spec ( \r\n )
        /// </summary>
        public const string LineBreak = "\r\n";
    }
}
