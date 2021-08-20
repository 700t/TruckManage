using System;
using System.IO;

namespace Qiniu.Util
{
    /// <summary>
    /// QINIU ETAG(文件hash)
    /// </summary>
    public class QETag
    {
        // 块大小(固定为4MB)
        private const int BLOCK_SIZE = 4 * 1024 * 1024;

        // 计算时以20B为单位
        //private static int BLOCK_SHA1_SIZE = 20;
    }
}