using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Assets.Scripts.PassDog
{
    public static class ET99_API
    {
        #region 常量

        /// <summary>
        /// 函数执行成功 
        /// </summary>
        public const int ET_SUCCESS = 0x00;

        /// <summary>
        /// 访问被拒绝，权限不够 
        /// </summary>
        public const int ET_ACCESS_DENY = 0x01;

        /// <summary>
        /// 通讯错误，没有打开设备 
        /// </summary>
        public const int ET_COMMUNICATIONS_ERROR = 0x02;

        /// <summary>
        /// 无效的参数，参数出错 
        /// </summary>
        public const int ET_INVALID_PARAMETER = 0x03;

        /// <summary>
        /// 没有设置 PID 
        /// </summary>
        public const int ET_NOT_SET_PID = 0x04;

        /// <summary>
        /// 打开指定的设备失败 
        /// </summary>
        public const int ET_UNIT_NOT_FOUND = 0x05;

        /// <summary>
        /// 硬件错误 
        /// </summary>
        public const int ET_HARD_ERROR = 0x06;

        /// <summary>
        /// 未知错误 
        /// </summary>
        public const int ET_UNKNOWN_ERROR = 0x07;

        /// <summary>
        /// 验证 PIN码掩码 
        /// </summary>
        public const int ET_PIN_ERR_MASK = 0x0F;

        /// <summary>
        /// 验证 PIN码,设备被锁定
        /// </summary>
        public const int ET_PIN_ERR_LOCKED = 0xF0;

        /// <summary>
        /// 验证 PIN码错误且永远不锁死 
        /// </summary>
        public const int ET_PIN_ERR_MAX = 0xFF;

        /// <summary>
        /// 表示验证普通用户 pin 
        /// </summary>
        public const int ET_VERIFY_USERPIN = 0;

        /// <summary>
        /// 表示验证超级用户 pin 
        /// </summary>
        public const int ET_VERIFY_SOPIN = 1;

        /// <summary>
        /// 表示数据区可读写 
        /// </summary>
        public const int Et_Users_WRITE_READ = 0;

        /// <summary>
        /// 表示数据区只允许读
        /// </summary>
        public const int Et_Users_READ_ONLY = 1;

        /// <summary>
        /// 常量 PID,默认的产品PID
        /// </summary>
        public const string CONST_PID = "ffffffff";


        #endregion

        /// <summary>
        /// 根据错误码显示错误提示内容
        /// </summary>
        /// <param name="result"></param>
        public static string ShowResultText(uint result)
        {
            switch (result)
            {
                case (ET_SUCCESS):
                    {
                        return "操作成功！";
                    }
                case (ET_ACCESS_DENY):
                    {
                        return "访问被拒绝，权限不够！";
                    }
                case (ET_COMMUNICATIONS_ERROR):
                    {
                        return "通讯错误，没有打开设备 ！";
                    }
                case (ET_INVALID_PARAMETER):
                    {
                        return "无效的参数，参数出错 ！";
                    }
                case (ET_NOT_SET_PID):
                    {
                        return "没有设置 PID ！";
                    }
                case (ET_UNIT_NOT_FOUND):
                    {
                        return "打开指定的设备失败！";
                    }
                case (ET_HARD_ERROR):
                    {
                        return "硬件错误！";
                    }
                case (ET_UNKNOWN_ERROR):
                    {
                        return "未知错误！";
                    }
                case (ET_PIN_ERR_MAX):
                    {
                        return "PIN码错误！请核实。";
                    }
                case (ET_PIN_ERR_LOCKED):
                    {
                        return "PIN码错误！设备已经被锁死。";
                    }
            }

            //输出剩余PIN验证次数
            if (result > ET_PIN_ERR_LOCKED && result < ET_PIN_ERR_MAX)
            {
                return "PIN码验证错误！剩余重试次数：" + (result - ET_PIN_ERR_LOCKED).ToString();
            }

            return "未知代码！";
        }

        /// <summary>
        /// 查找计算机上指定 pid 的 ET99 个数。
        /// </summary>
        /// <param name="pid">[in]产品标识,  为固定长度 8 个字节的字符串； </param>
        /// <param name="count">[out]还回的设备个数；</param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_FindToken(byte[] pid, out int count);

        /// <summary>
        /// 打开指定 PID的硬件,由 index 指定打开硬件的索引， index 应该小于等于找到的 Token 数目。进入匿名用户状态。
        /// </summary>
        /// <param name="hHandle">[out]打开设备的句柄，返回给用户，供以后的函数调用；</param>
        /// <param name="pid">[in]输入的硬件设备的 pid,  为固定长度 8 个字节的字符串；</param>
        /// <param name="index">[in]打开第 index 个硬件设备。 </param>
        /// <returns>ET_SUCCESS：执行成功。 ET_UNIT_NOT_FOUND：打开指定的设备失败。</returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_OpenToken(ref IntPtr hHandle, byte[] pid, int index);

        /// <summary>
        /// 关闭指定的设备。 
        /// </summary>
        /// <param name="hHandle">[in] 设备句柄。</param>
        /// <returns>ET_SUCCESS：关闭成功。  ET_COMMUNICATIONS_ERROR：没有打开设备。</returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_CloseToken(IntPtr hHandle);

        /// <summary>
        /// 从指定的位置，读取指定的数据到指定的 BUFFER 中。此函数调用需要有User权限，且调用以后不改变安全状态。
        /// </summary>
        /// <param name="hHandle">：[in]设备句柄</param>
        /// <param name="offset">[in]偏移量 </param>
        /// <param name="len">[in]长度，不能超过 60，如果超过则需要读多次。</param>
        /// <param name="pucReadBuf">[out]读出的数据存放此缓存区中，调用者保证缓冲区大小至少是 Len，否则可能产生系统存取异常。 </param>
        /// <returns>第二章 API接口函数 ET_COMMUNICATIONS_ERROR：没有打开设备。ET_SUCCESS：表示成功。 ET_INVALID_PARAMETER：无效的参数。 ET_NOT_SET_PID：没有设置 PID。 ET_ACCESS_DENY：权限不够。 </returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_Read(IntPtr hHandle, ushort offset, int len, byte[] pucReadBuf);

        /// <summary>
        /// 将 buf 中，Length 长的数据写到指定的偏移。有存取权限控制。匿名状态不可用，且在普通用户状态时还需要检查设备的配置。不改变安全状态。
        /// </summary>
        /// <param name="hHandle">[in]设备句柄；</param>
        /// <param name="offset">[in]偏移；</param>
        /// <param name="len">[in]长度，不能超过 60，如果超过则需要写多次； </param>
        /// <param name="pucReadBuf">[in]等写入的数据缓存区指针； </param>
        /// <returns>ET_SUCCESS：表示成功。 ET_HARD_ERROR：硬件错误 ET_INVALID_PARAMETER：无效的参数。 ET_NOT_SET_PID：没有设置 PID。 ET_ACCESS_DENY：权限不够。 ET_COMMUNICATIONS_ERROR：没有打开设备。 </returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_Write(IntPtr hHandle, ushort offset, int len, byte[] pucWriteBuf);

        /// <summary>
        /// 根据参数中指定的种子，产生产品标识。种子长度不能超过 51 个字节。必须在超级用户状态下才能用，调用以后不改变安全状态。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄； </param>
        /// <param name="seedlen">[in]种子； </param>
        /// <param name="pucseed">[in]种子长度，小于等于 51； </param>
        /// <param name="pid">[out]产生的产品标识,  为固定长度 8 个字节的字符串； </param>
        /// <returns>ET_SUCCESS：表示成功； ET_HARD_ERROR：硬件错误 ET_INVALID_PARAMETER：无效的参数； ET_ACCESS_DENY：权限不够，需要先验证 SOPIN。 ET_COMMUNICATIONS_ERROR：没有打开设备。 </returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_GenPID(IntPtr hHandle, int seedlen, byte[] pucseed, StringBuilder pid);

        /// <summary>
        /// 产生 16 字节的随机数，放到参数指定的 BUF中。调用者需要保护 BUF至少16 字节，否则会产生系统的存取异常。该函数在匿名状态不可用，且在函数调用以后，安全状态不变。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="pucRandBuf">[out]等写入的数据缓存区指针 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_GenRandom(IntPtr hHandle, ref byte[] pucRandBuf);

        /// <summary>
        /// 产生超级用户 PIN 码 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="seedlen">[in]产生超级用户密码需要的种子。 </param>
        /// <param name="pucseed">[in]种子长度，小于等于 51 </param>
        /// <param name="pucNewSoPin">：[out]用于存放产生的超级用户密码的缓冲区指针，至少可容纳 16 字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_GenSOPIN(IntPtr hHandle, int seedlen, byte[] pucseed, StringBuilder pucNewSoPin);


        /// <summary>
        /// 重新设置普通用户密码为 16 个‘F’，相当于解锁。命令执行成功后，当前安全状态变成超级用户状态。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="pucSoPin">[in]超级用户密码，16 字节。</param>
        /// <returns>如果验证超级PIN码错误，并且错误值在0xF0和ET_PIN_ERR_MAX （0xFF）之间,我们可以通过错误码&ET_PIN_ERR_MASK(0x0F)得到剩余重试次数。如果还回 0xF0 表示已经被锁死，如果还回 0xFF 表示验证出错，且 pin 永远不被锁死</returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_ResetPIN(IntPtr hHandle, byte[] pucSoPin);


        /// <summary>
        /// 更新参数指定的密钥，此密钥是用于计算 HMAC－MD5 的。其中 KEY的获得，是通过一个纯软件接口 HMAC_MD5（），参见相应说明。匿名状态不可用，且在普通用户状态时还需要检查设备配置。不改变安全状态。
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="Keyid">[in]密钥指示，取值范围（1—8） </param>
        /// <param name="pucKeyBuf">[in]KEY缓存区指针, KEY固定为 32 字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_SetKey(IntPtr hHandle, int Keyid, byte[] pucKeyBuf);

        /// <summary>
        /// 标准 HMAC_MD5 的软件实现，参照 RFC2104 标准。
        /// </summary>
        /// <param name="pucText">[in]等处理的数据缓存区指针，大于 0 小于等于 51 个字节 </param>
        /// <param name="ulText_Len">[in]数据长度，大于 0 小于等于 51 </param>
        /// <param name="pucKey">[in]密钥，按标准 RFC2104，长度可以任意 </param>
        /// <param name="ulKey_Len">[in]密钥长度 </param>
        /// <param name="pucToenKey">[out]硬件计算需要的 KEY，固定 32 字节。 </param>
        /// <param name="pucDigest">[out]计算结果，固定 16 字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint MD5_HMAC(byte[] pucText, byte ulText_Len, byte[] pucKey, byte ulKey_Len, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 32)]byte[] pucToenKey, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 16)]byte[] pucDigest);


        /// <summary>
        /// 利用硬件计算 HMAC-MD5  ，pid 为出厂时，还回错误。权限等同于 KEY的读权限。不改变安全状态。
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="Keyid">[in]密钥指示，范围（1—8）</param>
        /// <param name="textLen">[in]待计算的数据，大于 0 小于等于 51 个字节 </param>
        /// <param name="pucText">[in]数据长度，大于 0 小于等于 51 </param>
        /// <param name="digest">[out]散列结果的数据指针，固定长度 16 个字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_HMAC_MD5(IntPtr hHandle, int Keyid, int textLen, byte[] pucText, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 16)]byte[] digest);

        /// <summary>
        /// 验证密码，以获得相应的安全状态，不受安全状态限制，验证成功以后，进入相应的安全状态。ET_VERIFY_USER_PIN = 验证的是普通用户PIN码，如果验证通过，则进入普通用户状态。
        /// </summary>
        /// <param name="hHandle">[in]设备句柄；</param>
        /// <param name="Flags">[in]验证 PIN的类型，见下表； </param>
        /// <param name="pucPIN">[in] PIN 码，固定长度 16 个字节。 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_Verify(IntPtr hHandle, int Flags, byte[] pucPIN);


        /// <summary>
        ///   修改普通用户密码，从 pucOldPIN，改为 pucNewPIN。普通用户密码长度固定为 16 字节。此命令可以在匿名状态下进行，命令执行成功后，进入普通用户状态。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄</param>
        /// <param name="pucOldPIN">[in]原来的密码，长度固定为 16 字节 </param>
        /// <param name="pucNewPIN">[in]新密码，长度固定为 16 字节 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_ChangeUserPIN(IntPtr hHandle, byte[] pucOldPIN, byte[] pucNewPIN);

        /// <summary>
        /// 重置安全状态，回到匿名用户状态。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_ResetSecurityState(IntPtr hHandle);


        /// <summary>
        /// 获得硬件序列号。可以在匿名状态下进行。不改变安全状态。
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="pucSN">[out]用于存放获得的序列号，长度固定为 8 字节 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_GetSN(IntPtr hHandle, byte[] pucSN);


        /// <summary>
        /// 对硬件进行配置。必须在超级用户状态下进行。不改变安全状态。 
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <param name="bSoPINRetries">[in]超级 PIN 码的重试次数，范围 0—15，0 表示永远不被锁死；</param>
        /// <param name="bUserPINRetries">：[in]用户 PIN 码的重试次数，范围 0—15，0 表示永远不被锁死； </param>
        /// <param name="bUserReadOnly">[in]读写/只读标注，如下表； </param>
        /// <param name="bBack">[in]保留字，必须为 0。</param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_SetupToken(IntPtr hHandle, byte bSoPINRetries, byte bUserPINRetries, byte bUserReadOnly, byte bBack);

        /// <summary>
        /// 打开 LED灯，使其变亮。匿名状态不可用，不改变安全状态。设备加电后，LED灯是常亮的
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_TurnOnLED(IntPtr hHandle);

        /// <summary>
        /// 关闭 LED灯，使其变亮。匿名状态不可用，不改变安全状态。设备加电后，LED灯是常亮的
        /// </summary>
        /// <param name="hHandle">[in]设备句柄 </param>
        /// <returns></returns>
        [DllImport("FT_ET99_API.dll")]
        public static extern uint et_TurnOffLED(IntPtr hHandle);

        public static void et_Write()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
