using System;

namespace stringprep
{
    public class RFC3454
    {

        /*
         * A.1 Unassigned code points in Unicode 3.2
         * 
         */
        public static readonly Prohibit[] A_1 = new Prohibit[]
        {
            new Prohibit('\x0221'),                                            /* 0221 */
            new Prohibit('\x0234', '\x024F'),                                       /* 0234-024F */
            new Prohibit('\x02AE', '\x02AF'),                                       /* 02AE-02AF */
            new Prohibit('\x02EF', '\x02FF'),                                       /* 02EF-02FF */
            new Prohibit('\x0350', '\x035F'),                                       /* 0350-035F */
            new Prohibit('\x0370', '\x0373'),                                       /* 0370-0373 */
            new Prohibit('\x0376', '\x0379'),                                       /* 0376-0379 */
            new Prohibit('\x037B', '\x037D'),                                       /* 037B-037D */
            new Prohibit('\x037F', '\x0383'),                                       /* 037F-0383 */
            new Prohibit('\x038B'),                                            /* 038B */
            new Prohibit('\x038D'),                                            /* 038D */
            new Prohibit('\x03A2'),                                            /* 03A2 */
            new Prohibit('\x03CF'),                                            /* 03CF */
            new Prohibit('\x03F7', '\x03FF'),                                       /* 03F7-03FF */
            new Prohibit('\x0487'),                                            /* 0487 */
            new Prohibit('\x04CF'),                                            /* 04CF */
            new Prohibit('\x04F6', '\x04F7'),                                       /* 04F6-04F7 */
            new Prohibit('\x04FA', '\x04FF'),                                       /* 04FA-04FF */
            new Prohibit('\x0510', '\x0530'),                                       /* 0510-0530 */
            new Prohibit('\x0557', '\x0558'),                                       /* 0557-0558 */
            new Prohibit('\x0560'),                                            /* 0560 */
            new Prohibit('\x0588'),                                            /* 0588 */
            new Prohibit('\x058B', '\x0590'),                                       /* 058B-0590 */
            new Prohibit('\x05A2'),                                            /* 05A2 */
            new Prohibit('\x05BA'),                                            /* 05BA */
            new Prohibit('\x05C5', '\x05CF'),                                       /* 05C5-05CF */
            new Prohibit('\x05EB', '\x05EF'),                                       /* 05EB-05EF */
            new Prohibit('\x05F5', '\x060B'),                                       /* 05F5-060B */
            new Prohibit('\x060D', '\x061A'),                                       /* 060D-061A */
            new Prohibit('\x061C', '\x061E'),                                       /* 061C-061E */
            new Prohibit('\x0620'),                                            /* 0620 */
            new Prohibit('\x063B', '\x063F'),                                       /* 063B-063F */
            new Prohibit('\x0656', '\x065F'),                                       /* 0656-065F */
            new Prohibit('\x06EE', '\x06EF'),                                       /* 06EE-06EF */
            new Prohibit('\x06FF'),                                            /* 06FF */
            new Prohibit('\x070E'),                                            /* 070E */
            new Prohibit('\x072D', '\x072F'),                                       /* 072D-072F */
            new Prohibit('\x074B', '\x077F'),                                       /* 074B-077F */
            new Prohibit('\x07B2', '\x0900'),                                       /* 07B2-0900 */
            new Prohibit('\x0904'),                                            /* 0904 */
            new Prohibit('\x093A', '\x093B'),                                       /* 093A-093B */
            new Prohibit('\x094E', '\x094F'),                                       /* 094E-094F */
            new Prohibit('\x0955', '\x0957'),                                       /* 0955-0957 */
            new Prohibit('\x0971', '\x0980'),                                       /* 0971-0980 */
            new Prohibit('\x0984'),                                            /* 0984 */
            new Prohibit('\x098D', '\x098E'),                                       /* 098D-098E */
            new Prohibit('\x0991', '\x0992'),                                       /* 0991-0992 */
            new Prohibit('\x09A9'),                                            /* 09A9 */
            new Prohibit('\x09B1'),                                            /* 09B1 */
            new Prohibit('\x09B3', '\x09B5'),                                       /* 09B3-09B5 */
            new Prohibit('\x09BA', '\x09BB'),                                       /* 09BA-09BB */
            new Prohibit('\x09BD'),                                            /* 09BD */
            new Prohibit('\x09C5', '\x09C6'),                                       /* 09C5-09C6 */
            new Prohibit('\x09C9', '\x09CA'),                                       /* 09C9-09CA */
            new Prohibit('\x09CE', '\x09D6'),                                       /* 09CE-09D6 */
            new Prohibit('\x09D8', '\x09DB'),                                       /* 09D8-09DB */
            new Prohibit('\x09DE'),                                            /* 09DE */
            new Prohibit('\x09E4', '\x09E5'),                                       /* 09E4-09E5 */
            new Prohibit('\x09FB', '\x0A01'),                                       /* 09FB-0A01 */
            new Prohibit('\x0A03', '\x0A04'),                                       /* 0A03-0A04 */
            new Prohibit('\x0A0B', '\x0A0E'),                                       /* 0A0B-0A0E */
            new Prohibit('\x0A11', '\x0A12'),                                       /* 0A11-0A12 */
            new Prohibit('\x0A29'),                                            /* 0A29 */
            new Prohibit('\x0A31'),                                            /* 0A31 */
            new Prohibit('\x0A34'),                                            /* 0A34 */
            new Prohibit('\x0A37'),                                            /* 0A37 */
            new Prohibit('\x0A3A', '\x0A3B'),                                       /* 0A3A-0A3B */
            new Prohibit('\x0A3D'),                                            /* 0A3D */
            new Prohibit('\x0A43', '\x0A46'),                                       /* 0A43-0A46 */
            new Prohibit('\x0A49', '\x0A4A'),                                       /* 0A49-0A4A */
            new Prohibit('\x0A4E', '\x0A58'),                                       /* 0A4E-0A58 */
            new Prohibit('\x0A5D'),                                            /* 0A5D */
            new Prohibit('\x0A5F', '\x0A65'),                                       /* 0A5F-0A65 */
            new Prohibit('\x0A75', '\x0A80'),                                       /* 0A75-0A80 */
            new Prohibit('\x0A84'),                                            /* 0A84 */
            new Prohibit('\x0A8C'),                                            /* 0A8C */
            new Prohibit('\x0A8E'),                                            /* 0A8E */
            new Prohibit('\x0A92'),                                            /* 0A92 */
            new Prohibit('\x0AA9'),                                            /* 0AA9 */
            new Prohibit('\x0AB1'),                                            /* 0AB1 */
            new Prohibit('\x0AB4'),                                            /* 0AB4 */
            new Prohibit('\x0ABA', '\x0ABB'),                                       /* 0ABA-0ABB */
            new Prohibit('\x0AC6'),                                            /* 0AC6 */
            new Prohibit('\x0ACA'),                                            /* 0ACA */
            new Prohibit('\x0ACE', '\x0ACF'),                                       /* 0ACE-0ACF */
            new Prohibit('\x0AD1', '\x0ADF'),                                       /* 0AD1-0ADF */
            new Prohibit('\x0AE1', '\x0AE5'),                                       /* 0AE1-0AE5 */
            new Prohibit('\x0AF0', '\x0B00'),                                       /* 0AF0-0B00 */
            new Prohibit('\x0B04'),                                            /* 0B04 */
            new Prohibit('\x0B0D', '\x0B0E'),                                       /* 0B0D-0B0E */
            new Prohibit('\x0B11', '\x0B12'),                                       /* 0B11-0B12 */
            new Prohibit('\x0B29'),                                            /* 0B29 */
            new Prohibit('\x0B31'),                                            /* 0B31 */
            new Prohibit('\x0B34', '\x0B35'),                                       /* 0B34-0B35 */
            new Prohibit('\x0B3A', '\x0B3B'),                                       /* 0B3A-0B3B */
            new Prohibit('\x0B44', '\x0B46'),                                       /* 0B44-0B46 */
            new Prohibit('\x0B49', '\x0B4A'),                                       /* 0B49-0B4A */
            new Prohibit('\x0B4E', '\x0B55'),                                       /* 0B4E-0B55 */
            new Prohibit('\x0B58', '\x0B5B'),                                       /* 0B58-0B5B */
            new Prohibit('\x0B5E'),                                            /* 0B5E */
            new Prohibit('\x0B62', '\x0B65'),                                       /* 0B62-0B65 */
            new Prohibit('\x0B71', '\x0B81'),                                       /* 0B71-0B81 */
            new Prohibit('\x0B84'),                                            /* 0B84 */
            new Prohibit('\x0B8B', '\x0B8D'),                                       /* 0B8B-0B8D */
            new Prohibit('\x0B91'),                                            /* 0B91 */
            new Prohibit('\x0B96', '\x0B98'),                                       /* 0B96-0B98 */
            new Prohibit('\x0B9B'),                                            /* 0B9B */
            new Prohibit('\x0B9D'),                                            /* 0B9D */
            new Prohibit('\x0BA0', '\x0BA2'),                                       /* 0BA0-0BA2 */
            new Prohibit('\x0BA5', '\x0BA7'),                                       /* 0BA5-0BA7 */
            new Prohibit('\x0BAB', '\x0BAD'),                                       /* 0BAB-0BAD */
            new Prohibit('\x0BB6'),                                            /* 0BB6 */
            new Prohibit('\x0BBA', '\x0BBD'),                                       /* 0BBA-0BBD */
            new Prohibit('\x0BC3', '\x0BC5'),                                       /* 0BC3-0BC5 */
            new Prohibit('\x0BC9'),                                            /* 0BC9 */
            new Prohibit('\x0BCE', '\x0BD6'),                                       /* 0BCE-0BD6 */
            new Prohibit('\x0BD8', '\x0BE6'),                                       /* 0BD8-0BE6 */
            new Prohibit('\x0BF3', '\x0C00'),                                       /* 0BF3-0C00 */
            new Prohibit('\x0C04'),                                            /* 0C04 */
            new Prohibit('\x0C0D'),                                            /* 0C0D */
            new Prohibit('\x0C11'),                                            /* 0C11 */
            new Prohibit('\x0C29'),                                            /* 0C29 */
            new Prohibit('\x0C34'),                                            /* 0C34 */
            new Prohibit('\x0C3A', '\x0C3D'),                                       /* 0C3A-0C3D */
            new Prohibit('\x0C45'),                                            /* 0C45 */
            new Prohibit('\x0C49'),                                            /* 0C49 */
            new Prohibit('\x0C4E', '\x0C54'),                                       /* 0C4E-0C54 */
            new Prohibit('\x0C57', '\x0C5F'),                                       /* 0C57-0C5F */
            new Prohibit('\x0C62', '\x0C65'),                                       /* 0C62-0C65 */
            new Prohibit('\x0C70', '\x0C81'),                                       /* 0C70-0C81 */
            new Prohibit('\x0C84'),                                            /* 0C84 */
            new Prohibit('\x0C8D'),                                            /* 0C8D */
            new Prohibit('\x0C91'),                                            /* 0C91 */
            new Prohibit('\x0CA9'),                                            /* 0CA9 */
            new Prohibit('\x0CB4'),                                            /* 0CB4 */
            new Prohibit('\x0CBA', '\x0CBD'),                                       /* 0CBA-0CBD */
            new Prohibit('\x0CC5'),                                            /* 0CC5 */
            new Prohibit('\x0CC9'),                                            /* 0CC9 */
            new Prohibit('\x0CCE', '\x0CD4'),                                       /* 0CCE-0CD4 */
            new Prohibit('\x0CD7', '\x0CDD'),                                       /* 0CD7-0CDD */
            new Prohibit('\x0CDF'),                                            /* 0CDF */
            new Prohibit('\x0CE2', '\x0CE5'),                                       /* 0CE2-0CE5 */
            new Prohibit('\x0CF0', '\x0D01'),                                       /* 0CF0-0D01 */
            new Prohibit('\x0D04'),                                            /* 0D04 */
            new Prohibit('\x0D0D'),                                            /* 0D0D */
            new Prohibit('\x0D11'),                                            /* 0D11 */
            new Prohibit('\x0D29'),                                            /* 0D29 */
            new Prohibit('\x0D3A', '\x0D3D'),                                       /* 0D3A-0D3D */
            new Prohibit('\x0D44', '\x0D45'),                                       /* 0D44-0D45 */
            new Prohibit('\x0D49'),                                            /* 0D49 */
            new Prohibit('\x0D4E', '\x0D56'),                                       /* 0D4E-0D56 */
            new Prohibit('\x0D58', '\x0D5F'),                                       /* 0D58-0D5F */
            new Prohibit('\x0D62', '\x0D65'),                                       /* 0D62-0D65 */
            new Prohibit('\x0D70', '\x0D81'),                                       /* 0D70-0D81 */
            new Prohibit('\x0D84'),                                            /* 0D84 */
            new Prohibit('\x0D97', '\x0D99'),                                       /* 0D97-0D99 */
            new Prohibit('\x0DB2'),                                            /* 0DB2 */
            new Prohibit('\x0DBC'),                                            /* 0DBC */
            new Prohibit('\x0DBE', '\x0DBF'),                                       /* 0DBE-0DBF */
            new Prohibit('\x0DC7', '\x0DC9'),                                       /* 0DC7-0DC9 */
            new Prohibit('\x0DCB', '\x0DCE'),                                       /* 0DCB-0DCE */
            new Prohibit('\x0DD5'),                                            /* 0DD5 */
            new Prohibit('\x0DD7'),                                            /* 0DD7 */
            new Prohibit('\x0DE0', '\x0DF1'),                                       /* 0DE0-0DF1 */
            new Prohibit('\x0DF5', '\x0E00'),                                       /* 0DF5-0E00 */
            new Prohibit('\x0E3B', '\x0E3E'),                                       /* 0E3B-0E3E */
            new Prohibit('\x0E5C', '\x0E80'),                                       /* 0E5C-0E80 */
            new Prohibit('\x0E83'),                                            /* 0E83 */
            new Prohibit('\x0E85', '\x0E86'),                                       /* 0E85-0E86 */
            new Prohibit('\x0E89'),                                            /* 0E89 */
            new Prohibit('\x0E8B', '\x0E8C'),                                       /* 0E8B-0E8C */
            new Prohibit('\x0E8E', '\x0E93'),                                       /* 0E8E-0E93 */
            new Prohibit('\x0E98'),                                            /* 0E98 */
            new Prohibit('\x0EA0'),                                            /* 0EA0 */
            new Prohibit('\x0EA4'),                                            /* 0EA4 */
            new Prohibit('\x0EA6'),                                            /* 0EA6 */
            new Prohibit('\x0EA8', '\x0EA9'),                                       /* 0EA8-0EA9 */
            new Prohibit('\x0EAC'),                                            /* 0EAC */
            new Prohibit('\x0EBA'),                                            /* 0EBA */
            new Prohibit('\x0EBE', '\x0EBF'),                                       /* 0EBE-0EBF */
            new Prohibit('\x0EC5'),                                            /* 0EC5 */
            new Prohibit('\x0EC7'),                                            /* 0EC7 */
            new Prohibit('\x0ECE', '\x0ECF'),                                       /* 0ECE-0ECF */
            new Prohibit('\x0EDA', '\x0EDB'),                                       /* 0EDA-0EDB */
            new Prohibit('\x0EDE', '\x0EFF'),                                       /* 0EDE-0EFF */
            new Prohibit('\x0F48'),                                            /* 0F48 */
            new Prohibit('\x0F6B', '\x0F70'),                                       /* 0F6B-0F70 */
            new Prohibit('\x0F8C', '\x0F8F'),                                       /* 0F8C-0F8F */
            new Prohibit('\x0F98'),                                            /* 0F98 */
            new Prohibit('\x0FBD'),                                            /* 0FBD */
            new Prohibit('\x0FCD', '\x0FCE'),                                       /* 0FCD-0FCE */
            new Prohibit('\x0FD0', '\x0FFF'),                                       /* 0FD0-0FFF */
            new Prohibit('\x1022'),                                            /* 1022 */
            new Prohibit('\x1028'),                                            /* 1028 */
            new Prohibit('\x102B'),                                            /* 102B */
            new Prohibit('\x1033', '\x1035'),                                       /* 1033-1035 */
            new Prohibit('\x103A', '\x103F'),                                       /* 103A-103F */
            new Prohibit('\x105A', '\x109F'),                                       /* 105A-109F */
            new Prohibit('\x10C6', '\x10CF'),                                       /* 10C6-10CF */
            new Prohibit('\x10F9', '\x10FA'),                                       /* 10F9-10FA */
            new Prohibit('\x10FC', '\x10FF'),                                       /* 10FC-10FF */
            new Prohibit('\x115A', '\x115E'),                                       /* 115A-115E */
            new Prohibit('\x11A3', '\x11A7'),                                       /* 11A3-11A7 */
            new Prohibit('\x11FA', '\x11FF'),                                       /* 11FA-11FF */
            new Prohibit('\x1207'),                                            /* 1207 */
            new Prohibit('\x1247'),                                            /* 1247 */
            new Prohibit('\x1249'),                                            /* 1249 */
            new Prohibit('\x124E', '\x124F'),                                       /* 124E-124F */
            new Prohibit('\x1257'),                                            /* 1257 */
            new Prohibit('\x1259'),                                            /* 1259 */
            new Prohibit('\x125E', '\x125F'),                                       /* 125E-125F */
            new Prohibit('\x1287'),                                            /* 1287 */
            new Prohibit('\x1289'),                                            /* 1289 */
            new Prohibit('\x128E', '\x128F'),                                       /* 128E-128F */
            new Prohibit('\x12AF'),                                            /* 12AF */
            new Prohibit('\x12B1'),                                            /* 12B1 */
            new Prohibit('\x12B6', '\x12B7'),                                       /* 12B6-12B7 */
            new Prohibit('\x12BF'),                                            /* 12BF */
            new Prohibit('\x12C1'),                                            /* 12C1 */
            new Prohibit('\x12C6', '\x12C7'),                                       /* 12C6-12C7 */
            new Prohibit('\x12CF'),                                            /* 12CF */
            new Prohibit('\x12D7'),                                            /* 12D7 */
            new Prohibit('\x12EF'),                                            /* 12EF */
            new Prohibit('\x130F'),                                            /* 130F */
            new Prohibit('\x1311'),                                            /* 1311 */
            new Prohibit('\x1316', '\x1317'),                                       /* 1316-1317 */
            new Prohibit('\x131F'),                                            /* 131F */
            new Prohibit('\x1347'),                                            /* 1347 */
            new Prohibit('\x135B', '\x1360'),                                       /* 135B-1360 */
            new Prohibit('\x137D', '\x139F'),                                       /* 137D-139F */
            new Prohibit('\x13F5', '\x1400'),                                       /* 13F5-1400 */
            new Prohibit('\x1677', '\x167F'),                                       /* 1677-167F */
            new Prohibit('\x169D', '\x169F'),                                       /* 169D-169F */
            new Prohibit('\x16F1', '\x16FF'),                                       /* 16F1-16FF */
            new Prohibit('\x170D'),                                            /* 170D */
            new Prohibit('\x1715', '\x171F'),                                       /* 1715-171F */
            new Prohibit('\x1737', '\x173F'),                                       /* 1737-173F */
            new Prohibit('\x1754', '\x175F'),                                       /* 1754-175F */
            new Prohibit('\x176D'),                                            /* 176D */
            new Prohibit('\x1771'),                                            /* 1771 */
            new Prohibit('\x1774', '\x177F'),                                       /* 1774-177F */
            new Prohibit('\x17DD', '\x17DF'),                                       /* 17DD-17DF */
            new Prohibit('\x17EA', '\x17FF'),                                       /* 17EA-17FF */
            new Prohibit('\x180F'),                                            /* 180F */
            new Prohibit('\x181A', '\x181F'),                                       /* 181A-181F */
            new Prohibit('\x1878', '\x187F'),                                       /* 1878-187F */
            new Prohibit('\x18AA', '\x1DFF'),                                       /* 18AA-1DFF */
            new Prohibit('\x1E9C', '\x1E9F'),                                       /* 1E9C-1E9F */
            new Prohibit('\x1EFA', '\x1EFF'),                                       /* 1EFA-1EFF */
            new Prohibit('\x1F16', '\x1F17'),                                       /* 1F16-1F17 */
            new Prohibit('\x1F1E', '\x1F1F'),                                       /* 1F1E-1F1F */
            new Prohibit('\x1F46', '\x1F47'),                                       /* 1F46-1F47 */
            new Prohibit('\x1F4E', '\x1F4F'),                                       /* 1F4E-1F4F */
            new Prohibit('\x1F58'),                                            /* 1F58 */
            new Prohibit('\x1F5A'),                                            /* 1F5A */
            new Prohibit('\x1F5C'),                                            /* 1F5C */
            new Prohibit('\x1F5E'),                                            /* 1F5E */
            new Prohibit('\x1F7E', '\x1F7F'),                                       /* 1F7E-1F7F */
            new Prohibit('\x1FB5'),                                            /* 1FB5 */
            new Prohibit('\x1FC5'),                                            /* 1FC5 */
            new Prohibit('\x1FD4', '\x1FD5'),                                       /* 1FD4-1FD5 */
            new Prohibit('\x1FDC'),                                            /* 1FDC */
            new Prohibit('\x1FF0', '\x1FF1'),                                       /* 1FF0-1FF1 */
            new Prohibit('\x1FF5'),                                            /* 1FF5 */
            new Prohibit('\x1FFF'),                                            /* 1FFF */
            new Prohibit('\x2053', '\x2056'),                                       /* 2053-2056 */
            new Prohibit('\x2058', '\x205E'),                                       /* 2058-205E */
            new Prohibit('\x2064', '\x2069'),                                       /* 2064-2069 */
            new Prohibit('\x2072', '\x2073'),                                       /* 2072-2073 */
            new Prohibit('\x208F', '\x209F'),                                       /* 208F-209F */
            new Prohibit('\x20B2', '\x20CF'),                                       /* 20B2-20CF */
            new Prohibit('\x20EB', '\x20FF'),                                       /* 20EB-20FF */
            new Prohibit('\x213B', '\x213C'),                                       /* 213B-213C */
            new Prohibit('\x214C', '\x2152'),                                       /* 214C-2152 */
            new Prohibit('\x2184', '\x218F'),                                       /* 2184-218F */
            new Prohibit('\x23CF', '\x23FF'),                                       /* 23CF-23FF */
            new Prohibit('\x2427', '\x243F'),                                       /* 2427-243F */
            new Prohibit('\x244B', '\x245F'),                                       /* 244B-245F */
            new Prohibit('\x24FF'),                                            /* 24FF */
            new Prohibit('\x2614', '\x2615'),                                       /* 2614-2615 */
            new Prohibit('\x2618'),                                            /* 2618 */
            new Prohibit('\x267E', '\x267F'),                                       /* 267E-267F */
            new Prohibit('\x268A', '\x2700'),                                       /* 268A-2700 */
            new Prohibit('\x2705'),                                            /* 2705 */
            new Prohibit('\x270A', '\x270B'),                                       /* 270A-270B */
            new Prohibit('\x2728'),                                            /* 2728 */
            new Prohibit('\x274C'),                                            /* 274C */
            new Prohibit('\x274E'),                                            /* 274E */
            new Prohibit('\x2753', '\x2755'),                                       /* 2753-2755 */
            new Prohibit('\x2757'),                                            /* 2757 */
            new Prohibit('\x275F', '\x2760'),                                       /* 275F-2760 */
            new Prohibit('\x2795', '\x2797'),                                       /* 2795-2797 */
            new Prohibit('\x27B0'),                                            /* 27B0 */
            new Prohibit('\x27BF', '\x27CF'),                                       /* 27BF-27CF */
            new Prohibit('\x27EC', '\x27EF'),                                       /* 27EC-27EF */
            new Prohibit('\x2B00', '\x2E7F'),                                       /* 2B00-2E7F */
            new Prohibit('\x2E9A'),                                            /* 2E9A */
            new Prohibit('\x2EF4', '\x2EFF'),                                       /* 2EF4-2EFF */
            new Prohibit('\x2FD6', '\x2FEF'),                                       /* 2FD6-2FEF */
            new Prohibit('\x2FFC', '\x2FFF'),                                       /* 2FFC-2FFF */
            new Prohibit('\x3040'),                                            /* 3040 */
            new Prohibit('\x3097', '\x3098'),                                       /* 3097-3098 */
            new Prohibit('\x3100', '\x3104'),                                       /* 3100-3104 */
            new Prohibit('\x312D', '\x3130'),                                       /* 312D-3130 */
            new Prohibit('\x318F'),                                            /* 318F */
            new Prohibit('\x31B8', '\x31EF'),                                       /* 31B8-31EF */
            new Prohibit('\x321D', '\x321F'),                                       /* 321D-321F */
            new Prohibit('\x3244', '\x3250'),                                       /* 3244-3250 */
            new Prohibit('\x327C', '\x327E'),                                       /* 327C-327E */
            new Prohibit('\x32CC', '\x32CF'),                                       /* 32CC-32CF */
            new Prohibit('\x32FF'),                                            /* 32FF */
            new Prohibit('\x3377', '\x337A'),                                       /* 3377-337A */
            new Prohibit('\x33DE', '\x33DF'),                                       /* 33DE-33DF */
            new Prohibit('\x33FF'),                                            /* 33FF */
            new Prohibit('\x4DB6', '\x4DFF'),                                       /* 4DB6-4DFF */
            new Prohibit('\x9FA6', '\x9FFF'),                                       /* 9FA6-9FFF */
            new Prohibit('\xA48D', '\xA48F'),                                       /* A48D-A48F */
            new Prohibit('\xA4C7', '\xABFF'),                                       /* A4C7-ABFF */
            new Prohibit('\xD7A4', '\xD7FF'),                                       /* D7A4-D7FF */
            new Prohibit('\xFA2E', '\xFA2F'),                                       /* FA2E-FA2F */
            new Prohibit('\xFA6B', '\xFAFF'),                                       /* FA6B-FAFF */
            new Prohibit('\xFB07', '\xFB12'),                                       /* FB07-FB12 */
            new Prohibit('\xFB18', '\xFB1C'),                                       /* FB18-FB1C */
            new Prohibit('\xFB37'),                                            /* FB37 */
            new Prohibit('\xFB3D'),                                            /* FB3D */
            new Prohibit('\xFB3F'),                                            /* FB3F */
            new Prohibit('\xFB42'),                                            /* FB42 */
            new Prohibit('\xFB45'),                                            /* FB45 */
            new Prohibit('\xFBB2', '\xFBD2'),                                       /* FBB2-FBD2 */
            new Prohibit('\xFD40', '\xFD4F'),                                       /* FD40-FD4F */
            new Prohibit('\xFD90', '\xFD91'),                                       /* FD90-FD91 */
            new Prohibit('\xFDC8', '\xFDCF'),                                       /* FDC8-FDCF */
            new Prohibit('\xFDFD', '\xFDFF'),                                       /* FDFD-FDFF */
            new Prohibit('\xFE10', '\xFE1F'),                                       /* FE10-FE1F */
            new Prohibit('\xFE24', '\xFE2F'),                                       /* FE24-FE2F */
            new Prohibit('\xFE47', '\xFE48'),                                       /* FE47-FE48 */
            new Prohibit('\xFE53'),                                            /* FE53 */
            new Prohibit('\xFE67'),                                            /* FE67 */
            new Prohibit('\xFE6C', '\xFE6F'),                                       /* FE6C-FE6F */
            new Prohibit('\xFE75'),                                            /* FE75 */
            new Prohibit('\xFEFD', '\xFEFE'),                                       /* FEFD-FEFE */
            new Prohibit('\xFF00'),                                            /* FF00 */
            new Prohibit('\xFFBF', '\xFFC1'),                                       /* FFBF-FFC1 */
            new Prohibit('\xFFC8', '\xFFC9'),                                       /* FFC8-FFC9 */
            new Prohibit('\xFFD0', '\xFFD1'),                                       /* FFD0-FFD1 */
            new Prohibit('\xFFD8', '\xFFD9'),                                       /* FFD8-FFD9 */
            new Prohibit('\xFFDD', '\xFFDF'),                                       /* FFDD-FFDF */
            new Prohibit('\xFFE7'),                                            /* FFE7 */
            new Prohibit('\xFFEF', '\xFFF8'),                                       /* FFEF-FFF8 */
        };


        /*
         * B.1 Commonly mapped to nothing
         * 
         */
        public static readonly CharMap[] B_1 = new CharMap[]
        {
            new CharMap('\x00AD'),                          /* 00AD; ; Map to nothing */
            new CharMap('\x034F'),                          /* 034F; ; Map to nothing */
            new CharMap('\x1806'),                          /* 1806; ; Map to nothing */
            new CharMap('\x180B'),                          /* 180B; ; Map to nothing */
            new CharMap('\x180C'),                          /* 180C; ; Map to nothing */
            new CharMap('\x180D'),                          /* 180D; ; Map to nothing */
            new CharMap('\x200B'),                          /* 200B; ; Map to nothing */
            new CharMap('\x200C'),                          /* 200C; ; Map to nothing */
            new CharMap('\x200D'),                          /* 200D; ; Map to nothing */
            new CharMap('\x2060'),                          /* 2060; ; Map to nothing */
            new CharMap('\xFE00'),                          /* FE00; ; Map to nothing */
            new CharMap('\xFE01'),                          /* FE01; ; Map to nothing */
            new CharMap('\xFE02'),                          /* FE02; ; Map to nothing */
            new CharMap('\xFE03'),                          /* FE03; ; Map to nothing */
            new CharMap('\xFE04'),                          /* FE04; ; Map to nothing */
            new CharMap('\xFE05'),                          /* FE05; ; Map to nothing */
            new CharMap('\xFE06'),                          /* FE06; ; Map to nothing */
            new CharMap('\xFE07'),                          /* FE07; ; Map to nothing */
            new CharMap('\xFE08'),                          /* FE08; ; Map to nothing */
            new CharMap('\xFE09'),                          /* FE09; ; Map to nothing */
            new CharMap('\xFE0A'),                          /* FE0A; ; Map to nothing */
            new CharMap('\xFE0B'),                          /* FE0B; ; Map to nothing */
            new CharMap('\xFE0C'),                          /* FE0C; ; Map to nothing */
            new CharMap('\xFE0D'),                          /* FE0D; ; Map to nothing */
            new CharMap('\xFE0E'),                          /* FE0E; ; Map to nothing */
            new CharMap('\xFE0F'),                          /* FE0F; ; Map to nothing */
            new CharMap('\xFEFF'),                          /* FEFF; ; Map to nothing */
        };


        /*
         * B.2 Mapping for case-folding used with NFKC
         * 
         */
        public static readonly CharMap[] B_2 = new CharMap[]
        {
            new CharMap('\x0041', new char[] { '\x0061' }),                      /* 0041; 0061; Case map */
            new CharMap('\x0042', new char[] { '\x0062' }),                      /* 0042; 0062; Case map */
            new CharMap('\x0043', new char[] { '\x0063' }),                      /* 0043; 0063; Case map */
            new CharMap('\x0044', new char[] { '\x0064' }),                      /* 0044; 0064; Case map */
            new CharMap('\x0045', new char[] { '\x0065' }),                      /* 0045; 0065; Case map */
            new CharMap('\x0046', new char[] { '\x0066' }),                      /* 0046; 0066; Case map */
            new CharMap('\x0047', new char[] { '\x0067' }),                      /* 0047; 0067; Case map */
            new CharMap('\x0048', new char[] { '\x0068' }),                      /* 0048; 0068; Case map */
            new CharMap('\x0049', new char[] { '\x0069' }),                      /* 0049; 0069; Case map */
            new CharMap('\x004A', new char[] { '\x006A' }),                      /* 004A; 006A; Case map */
            new CharMap('\x004B', new char[] { '\x006B' }),                      /* 004B; 006B; Case map */
            new CharMap('\x004C', new char[] { '\x006C' }),                      /* 004C; 006C; Case map */
            new CharMap('\x004D', new char[] { '\x006D' }),                      /* 004D; 006D; Case map */
            new CharMap('\x004E', new char[] { '\x006E' }),                      /* 004E; 006E; Case map */
            new CharMap('\x004F', new char[] { '\x006F' }),                      /* 004F; 006F; Case map */
            new CharMap('\x0050', new char[] { '\x0070' }),                      /* 0050; 0070; Case map */
            new CharMap('\x0051', new char[] { '\x0071' }),                      /* 0051; 0071; Case map */
            new CharMap('\x0052', new char[] { '\x0072' }),                      /* 0052; 0072; Case map */
            new CharMap('\x0053', new char[] { '\x0073' }),                      /* 0053; 0073; Case map */
            new CharMap('\x0054', new char[] { '\x0074' }),                      /* 0054; 0074; Case map */
            new CharMap('\x0055', new char[] { '\x0075' }),                      /* 0055; 0075; Case map */
            new CharMap('\x0056', new char[] { '\x0076' }),                      /* 0056; 0076; Case map */
            new CharMap('\x0057', new char[] { '\x0077' }),                      /* 0057; 0077; Case map */
            new CharMap('\x0058', new char[] { '\x0078' }),                      /* 0058; 0078; Case map */
            new CharMap('\x0059', new char[] { '\x0079' }),                      /* 0059; 0079; Case map */
            new CharMap('\x005A', new char[] { '\x007A' }),                      /* 005A; 007A; Case map */
            new CharMap('\x00B5', new char[] { '\x03BC' }),                      /* 00B5; 03BC; Case map */
            new CharMap('\x00C0', new char[] { '\x00E0' }),                      /* 00C0; 00E0; Case map */
            new CharMap('\x00C1', new char[] { '\x00E1' }),                      /* 00C1; 00E1; Case map */
            new CharMap('\x00C2', new char[] { '\x00E2' }),                      /* 00C2; 00E2; Case map */
            new CharMap('\x00C3', new char[] { '\x00E3' }),                      /* 00C3; 00E3; Case map */
            new CharMap('\x00C4', new char[] { '\x00E4' }),                      /* 00C4; 00E4; Case map */
            new CharMap('\x00C5', new char[] { '\x00E5' }),                      /* 00C5; 00E5; Case map */
            new CharMap('\x00C6', new char[] { '\x00E6' }),                      /* 00C6; 00E6; Case map */
            new CharMap('\x00C7', new char[] { '\x00E7' }),                      /* 00C7; 00E7; Case map */
            new CharMap('\x00C8', new char[] { '\x00E8' }),                      /* 00C8; 00E8; Case map */
            new CharMap('\x00C9', new char[] { '\x00E9' }),                      /* 00C9; 00E9; Case map */
            new CharMap('\x00CA', new char[] { '\x00EA' }),                      /* 00CA; 00EA; Case map */
            new CharMap('\x00CB', new char[] { '\x00EB' }),                      /* 00CB; 00EB; Case map */
            new CharMap('\x00CC', new char[] { '\x00EC' }),                      /* 00CC; 00EC; Case map */
            new CharMap('\x00CD', new char[] { '\x00ED' }),                      /* 00CD; 00ED; Case map */
            new CharMap('\x00CE', new char[] { '\x00EE' }),                      /* 00CE; 00EE; Case map */
            new CharMap('\x00CF', new char[] { '\x00EF' }),                      /* 00CF; 00EF; Case map */
            new CharMap('\x00D0', new char[] { '\x00F0' }),                      /* 00D0; 00F0; Case map */
            new CharMap('\x00D1', new char[] { '\x00F1' }),                      /* 00D1; 00F1; Case map */
            new CharMap('\x00D2', new char[] { '\x00F2' }),                      /* 00D2; 00F2; Case map */
            new CharMap('\x00D3', new char[] { '\x00F3' }),                      /* 00D3; 00F3; Case map */
            new CharMap('\x00D4', new char[] { '\x00F4' }),                      /* 00D4; 00F4; Case map */
            new CharMap('\x00D5', new char[] { '\x00F5' }),                      /* 00D5; 00F5; Case map */
            new CharMap('\x00D6', new char[] { '\x00F6' }),                      /* 00D6; 00F6; Case map */
            new CharMap('\x00D8', new char[] { '\x00F8' }),                      /* 00D8; 00F8; Case map */
            new CharMap('\x00D9', new char[] { '\x00F9' }),                      /* 00D9; 00F9; Case map */
            new CharMap('\x00DA', new char[] { '\x00FA' }),                      /* 00DA; 00FA; Case map */
            new CharMap('\x00DB', new char[] { '\x00FB' }),                      /* 00DB; 00FB; Case map */
            new CharMap('\x00DC', new char[] { '\x00FC' }),                      /* 00DC; 00FC; Case map */
            new CharMap('\x00DD', new char[] { '\x00FD' }),                      /* 00DD; 00FD; Case map */
            new CharMap('\x00DE', new char[] { '\x00FE' }),                      /* 00DE; 00FE; Case map */
            new CharMap('\x00DF', new char[] { '\x0073',                    /* 00DF; 0073 0073; Case map */
                   '\x0073' }),
            new CharMap('\x0100', new char[] { '\x0101' }),                      /* 0100; 0101; Case map */
            new CharMap('\x0102', new char[] { '\x0103' }),                      /* 0102; 0103; Case map */
            new CharMap('\x0104', new char[] { '\x0105' }),                      /* 0104; 0105; Case map */
            new CharMap('\x0106', new char[] { '\x0107' }),                      /* 0106; 0107; Case map */
            new CharMap('\x0108', new char[] { '\x0109' }),                      /* 0108; 0109; Case map */
            new CharMap('\x010A', new char[] { '\x010B' }),                      /* 010A; 010B; Case map */
            new CharMap('\x010C', new char[] { '\x010D' }),                      /* 010C; 010D; Case map */
            new CharMap('\x010E', new char[] { '\x010F' }),                      /* 010E; 010F; Case map */
            new CharMap('\x0110', new char[] { '\x0111' }),                      /* 0110; 0111; Case map */
            new CharMap('\x0112', new char[] { '\x0113' }),                      /* 0112; 0113; Case map */
            new CharMap('\x0114', new char[] { '\x0115' }),                      /* 0114; 0115; Case map */
            new CharMap('\x0116', new char[] { '\x0117' }),                      /* 0116; 0117; Case map */
            new CharMap('\x0118', new char[] { '\x0119' }),                      /* 0118; 0119; Case map */
            new CharMap('\x011A', new char[] { '\x011B' }),                      /* 011A; 011B; Case map */
            new CharMap('\x011C', new char[] { '\x011D' }),                      /* 011C; 011D; Case map */
            new CharMap('\x011E', new char[] { '\x011F' }),                      /* 011E; 011F; Case map */
            new CharMap('\x0120', new char[] { '\x0121' }),                      /* 0120; 0121; Case map */
            new CharMap('\x0122', new char[] { '\x0123' }),                      /* 0122; 0123; Case map */
            new CharMap('\x0124', new char[] { '\x0125' }),                      /* 0124; 0125; Case map */
            new CharMap('\x0126', new char[] { '\x0127' }),                      /* 0126; 0127; Case map */
            new CharMap('\x0128', new char[] { '\x0129' }),                      /* 0128; 0129; Case map */
            new CharMap('\x012A', new char[] { '\x012B' }),                      /* 012A; 012B; Case map */
            new CharMap('\x012C', new char[] { '\x012D' }),                      /* 012C; 012D; Case map */
            new CharMap('\x012E', new char[] { '\x012F' }),                      /* 012E; 012F; Case map */
            new CharMap('\x0130', new char[] { '\x0069',                    /* 0130; 0069 0307; Case map */
                   '\x0307' }),
            new CharMap('\x0132', new char[] { '\x0133' }),                      /* 0132; 0133; Case map */
            new CharMap('\x0134', new char[] { '\x0135' }),                      /* 0134; 0135; Case map */
            new CharMap('\x0136', new char[] { '\x0137' }),                      /* 0136; 0137; Case map */
            new CharMap('\x0139', new char[] { '\x013A' }),                      /* 0139; 013A; Case map */
            new CharMap('\x013B', new char[] { '\x013C' }),                      /* 013B; 013C; Case map */
            new CharMap('\x013D', new char[] { '\x013E' }),                      /* 013D; 013E; Case map */
            new CharMap('\x013F', new char[] { '\x0140' }),                      /* 013F; 0140; Case map */
            new CharMap('\x0141', new char[] { '\x0142' }),                      /* 0141; 0142; Case map */
            new CharMap('\x0143', new char[] { '\x0144' }),                      /* 0143; 0144; Case map */
            new CharMap('\x0145', new char[] { '\x0146' }),                      /* 0145; 0146; Case map */
            new CharMap('\x0147', new char[] { '\x0148' }),                      /* 0147; 0148; Case map */
            new CharMap('\x0149', new char[] { '\x02BC',                    /* 0149; 02BC 006E; Case map */
                   '\x006E' }),
            new CharMap('\x014A', new char[] { '\x014B' }),                      /* 014A; 014B; Case map */
            new CharMap('\x014C', new char[] { '\x014D' }),                      /* 014C; 014D; Case map */
            new CharMap('\x014E', new char[] { '\x014F' }),                      /* 014E; 014F; Case map */
            new CharMap('\x0150', new char[] { '\x0151' }),                      /* 0150; 0151; Case map */
            new CharMap('\x0152', new char[] { '\x0153' }),                      /* 0152; 0153; Case map */
            new CharMap('\x0154', new char[] { '\x0155' }),                      /* 0154; 0155; Case map */
            new CharMap('\x0156', new char[] { '\x0157' }),                      /* 0156; 0157; Case map */
            new CharMap('\x0158', new char[] { '\x0159' }),                      /* 0158; 0159; Case map */
            new CharMap('\x015A', new char[] { '\x015B' }),                      /* 015A; 015B; Case map */
            new CharMap('\x015C', new char[] { '\x015D' }),                      /* 015C; 015D; Case map */
            new CharMap('\x015E', new char[] { '\x015F' }),                      /* 015E; 015F; Case map */
            new CharMap('\x0160', new char[] { '\x0161' }),                      /* 0160; 0161; Case map */
            new CharMap('\x0162', new char[] { '\x0163' }),                      /* 0162; 0163; Case map */
            new CharMap('\x0164', new char[] { '\x0165' }),                      /* 0164; 0165; Case map */
            new CharMap('\x0166', new char[] { '\x0167' }),                      /* 0166; 0167; Case map */
            new CharMap('\x0168', new char[] { '\x0169' }),                      /* 0168; 0169; Case map */
            new CharMap('\x016A', new char[] { '\x016B' }),                      /* 016A; 016B; Case map */
            new CharMap('\x016C', new char[] { '\x016D' }),                      /* 016C; 016D; Case map */
            new CharMap('\x016E', new char[] { '\x016F' }),                      /* 016E; 016F; Case map */
            new CharMap('\x0170', new char[] { '\x0171' }),                      /* 0170; 0171; Case map */
            new CharMap('\x0172', new char[] { '\x0173' }),                      /* 0172; 0173; Case map */
            new CharMap('\x0174', new char[] { '\x0175' }),                      /* 0174; 0175; Case map */
            new CharMap('\x0176', new char[] { '\x0177' }),                      /* 0176; 0177; Case map */
            new CharMap('\x0178', new char[] { '\x00FF' }),                      /* 0178; 00FF; Case map */
            new CharMap('\x0179', new char[] { '\x017A' }),                      /* 0179; 017A; Case map */
            new CharMap('\x017B', new char[] { '\x017C' }),                      /* 017B; 017C; Case map */
            new CharMap('\x017D', new char[] { '\x017E' }),                      /* 017D; 017E; Case map */
            new CharMap('\x017F', new char[] { '\x0073' }),                      /* 017F; 0073; Case map */
            new CharMap('\x0181', new char[] { '\x0253' }),                      /* 0181; 0253; Case map */
            new CharMap('\x0182', new char[] { '\x0183' }),                      /* 0182; 0183; Case map */
            new CharMap('\x0184', new char[] { '\x0185' }),                      /* 0184; 0185; Case map */
            new CharMap('\x0186', new char[] { '\x0254' }),                      /* 0186; 0254; Case map */
            new CharMap('\x0187', new char[] { '\x0188' }),                      /* 0187; 0188; Case map */
            new CharMap('\x0189', new char[] { '\x0256' }),                      /* 0189; 0256; Case map */
            new CharMap('\x018A', new char[] { '\x0257' }),                      /* 018A; 0257; Case map */
            new CharMap('\x018B', new char[] { '\x018C' }),                      /* 018B; 018C; Case map */
            new CharMap('\x018E', new char[] { '\x01DD' }),                      /* 018E; 01DD; Case map */
            new CharMap('\x018F', new char[] { '\x0259' }),                      /* 018F; 0259; Case map */
            new CharMap('\x0190', new char[] { '\x025B' }),                      /* 0190; 025B; Case map */
            new CharMap('\x0191', new char[] { '\x0192' }),                      /* 0191; 0192; Case map */
            new CharMap('\x0193', new char[] { '\x0260' }),                      /* 0193; 0260; Case map */
            new CharMap('\x0194', new char[] { '\x0263' }),                      /* 0194; 0263; Case map */
            new CharMap('\x0196', new char[] { '\x0269' }),                      /* 0196; 0269; Case map */
            new CharMap('\x0197', new char[] { '\x0268' }),                      /* 0197; 0268; Case map */
            new CharMap('\x0198', new char[] { '\x0199' }),                      /* 0198; 0199; Case map */
            new CharMap('\x019C', new char[] { '\x026F' }),                      /* 019C; 026F; Case map */
            new CharMap('\x019D', new char[] { '\x0272' }),                      /* 019D; 0272; Case map */
            new CharMap('\x019F', new char[] { '\x0275' }),                      /* 019F; 0275; Case map */
            new CharMap('\x01A0', new char[] { '\x01A1' }),                      /* 01A0; 01A1; Case map */
            new CharMap('\x01A2', new char[] { '\x01A3' }),                      /* 01A2; 01A3; Case map */
            new CharMap('\x01A4', new char[] { '\x01A5' }),                      /* 01A4; 01A5; Case map */
            new CharMap('\x01A6', new char[] { '\x0280' }),                      /* 01A6; 0280; Case map */
            new CharMap('\x01A7', new char[] { '\x01A8' }),                      /* 01A7; 01A8; Case map */
            new CharMap('\x01A9', new char[] { '\x0283' }),                      /* 01A9; 0283; Case map */
            new CharMap('\x01AC', new char[] { '\x01AD' }),                      /* 01AC; 01AD; Case map */
            new CharMap('\x01AE', new char[] { '\x0288' }),                      /* 01AE; 0288; Case map */
            new CharMap('\x01AF', new char[] { '\x01B0' }),                      /* 01AF; 01B0; Case map */
            new CharMap('\x01B1', new char[] { '\x028A' }),                      /* 01B1; 028A; Case map */
            new CharMap('\x01B2', new char[] { '\x028B' }),                      /* 01B2; 028B; Case map */
            new CharMap('\x01B3', new char[] { '\x01B4' }),                      /* 01B3; 01B4; Case map */
            new CharMap('\x01B5', new char[] { '\x01B6' }),                      /* 01B5; 01B6; Case map */
            new CharMap('\x01B7', new char[] { '\x0292' }),                      /* 01B7; 0292; Case map */
            new CharMap('\x01B8', new char[] { '\x01B9' }),                      /* 01B8; 01B9; Case map */
            new CharMap('\x01BC', new char[] { '\x01BD' }),                      /* 01BC; 01BD; Case map */
            new CharMap('\x01C4', new char[] { '\x01C6' }),                      /* 01C4; 01C6; Case map */
            new CharMap('\x01C5', new char[] { '\x01C6' }),                      /* 01C5; 01C6; Case map */
            new CharMap('\x01C7', new char[] { '\x01C9' }),                      /* 01C7; 01C9; Case map */
            new CharMap('\x01C8', new char[] { '\x01C9' }),                      /* 01C8; 01C9; Case map */
            new CharMap('\x01CA', new char[] { '\x01CC' }),                      /* 01CA; 01CC; Case map */
            new CharMap('\x01CB', new char[] { '\x01CC' }),                      /* 01CB; 01CC; Case map */
            new CharMap('\x01CD', new char[] { '\x01CE' }),                      /* 01CD; 01CE; Case map */
            new CharMap('\x01CF', new char[] { '\x01D0' }),                      /* 01CF; 01D0; Case map */
            new CharMap('\x01D1', new char[] { '\x01D2' }),                      /* 01D1; 01D2; Case map */
            new CharMap('\x01D3', new char[] { '\x01D4' }),                      /* 01D3; 01D4; Case map */
            new CharMap('\x01D5', new char[] { '\x01D6' }),                      /* 01D5; 01D6; Case map */
            new CharMap('\x01D7', new char[] { '\x01D8' }),                      /* 01D7; 01D8; Case map */
            new CharMap('\x01D9', new char[] { '\x01DA' }),                      /* 01D9; 01DA; Case map */
            new CharMap('\x01DB', new char[] { '\x01DC' }),                      /* 01DB; 01DC; Case map */
            new CharMap('\x01DE', new char[] { '\x01DF' }),                      /* 01DE; 01DF; Case map */
            new CharMap('\x01E0', new char[] { '\x01E1' }),                      /* 01E0; 01E1; Case map */
            new CharMap('\x01E2', new char[] { '\x01E3' }),                      /* 01E2; 01E3; Case map */
            new CharMap('\x01E4', new char[] { '\x01E5' }),                      /* 01E4; 01E5; Case map */
            new CharMap('\x01E6', new char[] { '\x01E7' }),                      /* 01E6; 01E7; Case map */
            new CharMap('\x01E8', new char[] { '\x01E9' }),                      /* 01E8; 01E9; Case map */
            new CharMap('\x01EA', new char[] { '\x01EB' }),                      /* 01EA; 01EB; Case map */
            new CharMap('\x01EC', new char[] { '\x01ED' }),                      /* 01EC; 01ED; Case map */
            new CharMap('\x01EE', new char[] { '\x01EF' }),                      /* 01EE; 01EF; Case map */
            new CharMap('\x01F0', new char[] { '\x006A',                    /* 01F0; 006A 030C; Case map */
                   '\x030C' }),
            new CharMap('\x01F1', new char[] { '\x01F3' }),                      /* 01F1; 01F3; Case map */
            new CharMap('\x01F2', new char[] { '\x01F3' }),                      /* 01F2; 01F3; Case map */
            new CharMap('\x01F4', new char[] { '\x01F5' }),                      /* 01F4; 01F5; Case map */
            new CharMap('\x01F6', new char[] { '\x0195' }),                      /* 01F6; 0195; Case map */
            new CharMap('\x01F7', new char[] { '\x01BF' }),                      /* 01F7; 01BF; Case map */
            new CharMap('\x01F8', new char[] { '\x01F9' }),                      /* 01F8; 01F9; Case map */
            new CharMap('\x01FA', new char[] { '\x01FB' }),                      /* 01FA; 01FB; Case map */
            new CharMap('\x01FC', new char[] { '\x01FD' }),                      /* 01FC; 01FD; Case map */
            new CharMap('\x01FE', new char[] { '\x01FF' }),                      /* 01FE; 01FF; Case map */
            new CharMap('\x0200', new char[] { '\x0201' }),                      /* 0200; 0201; Case map */
            new CharMap('\x0202', new char[] { '\x0203' }),                      /* 0202; 0203; Case map */
            new CharMap('\x0204', new char[] { '\x0205' }),                      /* 0204; 0205; Case map */
            new CharMap('\x0206', new char[] { '\x0207' }),                      /* 0206; 0207; Case map */
            new CharMap('\x0208', new char[] { '\x0209' }),                      /* 0208; 0209; Case map */
            new CharMap('\x020A', new char[] { '\x020B' }),                      /* 020A; 020B; Case map */
            new CharMap('\x020C', new char[] { '\x020D' }),                      /* 020C; 020D; Case map */
            new CharMap('\x020E', new char[] { '\x020F' }),                      /* 020E; 020F; Case map */
            new CharMap('\x0210', new char[] { '\x0211' }),                      /* 0210; 0211; Case map */
            new CharMap('\x0212', new char[] { '\x0213' }),                      /* 0212; 0213; Case map */
            new CharMap('\x0214', new char[] { '\x0215' }),                      /* 0214; 0215; Case map */
            new CharMap('\x0216', new char[] { '\x0217' }),                      /* 0216; 0217; Case map */
            new CharMap('\x0218', new char[] { '\x0219' }),                      /* 0218; 0219; Case map */
            new CharMap('\x021A', new char[] { '\x021B' }),                      /* 021A; 021B; Case map */
            new CharMap('\x021C', new char[] { '\x021D' }),                      /* 021C; 021D; Case map */
            new CharMap('\x021E', new char[] { '\x021F' }),                      /* 021E; 021F; Case map */
            new CharMap('\x0220', new char[] { '\x019E' }),                      /* 0220; 019E; Case map */
            new CharMap('\x0222', new char[] { '\x0223' }),                      /* 0222; 0223; Case map */
            new CharMap('\x0224', new char[] { '\x0225' }),                      /* 0224; 0225; Case map */
            new CharMap('\x0226', new char[] { '\x0227' }),                      /* 0226; 0227; Case map */
            new CharMap('\x0228', new char[] { '\x0229' }),                      /* 0228; 0229; Case map */
            new CharMap('\x022A', new char[] { '\x022B' }),                      /* 022A; 022B; Case map */
            new CharMap('\x022C', new char[] { '\x022D' }),                      /* 022C; 022D; Case map */
            new CharMap('\x022E', new char[] { '\x022F' }),                      /* 022E; 022F; Case map */
            new CharMap('\x0230', new char[] { '\x0231' }),                      /* 0230; 0231; Case map */
            new CharMap('\x0232', new char[] { '\x0233' }),                      /* 0232; 0233; Case map */
            new CharMap('\x0345', new char[] { '\x03B9' }),                      /* 0345; 03B9; Case map */
            new CharMap('\x037A', new char[] { '\x0020',          /* 037A; 0020 03B9; Additional folding */
                   '\x03B9' }),
            new CharMap('\x0386', new char[] { '\x03AC' }),                      /* 0386; 03AC; Case map */
            new CharMap('\x0388', new char[] { '\x03AD' }),                      /* 0388; 03AD; Case map */
            new CharMap('\x0389', new char[] { '\x03AE' }),                      /* 0389; 03AE; Case map */
            new CharMap('\x038A', new char[] { '\x03AF' }),                      /* 038A; 03AF; Case map */
            new CharMap('\x038C', new char[] { '\x03CC' }),                      /* 038C; 03CC; Case map */
            new CharMap('\x038E', new char[] { '\x03CD' }),                      /* 038E; 03CD; Case map */
            new CharMap('\x038F', new char[] { '\x03CE' }),                      /* 038F; 03CE; Case map */
            new CharMap('\x0390', new char[] { '\x03B9',               /* 0390; 03B9 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x0391', new char[] { '\x03B1' }),                      /* 0391; 03B1; Case map */
            new CharMap('\x0392', new char[] { '\x03B2' }),                      /* 0392; 03B2; Case map */
            new CharMap('\x0393', new char[] { '\x03B3' }),                      /* 0393; 03B3; Case map */
            new CharMap('\x0394', new char[] { '\x03B4' }),                      /* 0394; 03B4; Case map */
            new CharMap('\x0395', new char[] { '\x03B5' }),                      /* 0395; 03B5; Case map */
            new CharMap('\x0396', new char[] { '\x03B6' }),                      /* 0396; 03B6; Case map */
            new CharMap('\x0397', new char[] { '\x03B7' }),                      /* 0397; 03B7; Case map */
            new CharMap('\x0398', new char[] { '\x03B8' }),                      /* 0398; 03B8; Case map */
            new CharMap('\x0399', new char[] { '\x03B9' }),                      /* 0399; 03B9; Case map */
            new CharMap('\x039A', new char[] { '\x03BA' }),                      /* 039A; 03BA; Case map */
            new CharMap('\x039B', new char[] { '\x03BB' }),                      /* 039B; 03BB; Case map */
            new CharMap('\x039C', new char[] { '\x03BC' }),                      /* 039C; 03BC; Case map */
            new CharMap('\x039D', new char[] { '\x03BD' }),                      /* 039D; 03BD; Case map */
            new CharMap('\x039E', new char[] { '\x03BE' }),                      /* 039E; 03BE; Case map */
            new CharMap('\x039F', new char[] { '\x03BF' }),                      /* 039F; 03BF; Case map */
            new CharMap('\x03A0', new char[] { '\x03C0' }),                      /* 03A0; 03C0; Case map */
            new CharMap('\x03A1', new char[] { '\x03C1' }),                      /* 03A1; 03C1; Case map */
            new CharMap('\x03A3', new char[] { '\x03C3' }),                      /* 03A3; 03C3; Case map */
            new CharMap('\x03A4', new char[] { '\x03C4' }),                      /* 03A4; 03C4; Case map */
            new CharMap('\x03A5', new char[] { '\x03C5' }),                      /* 03A5; 03C5; Case map */
            new CharMap('\x03A6', new char[] { '\x03C6' }),                      /* 03A6; 03C6; Case map */
            new CharMap('\x03A7', new char[] { '\x03C7' }),                      /* 03A7; 03C7; Case map */
            new CharMap('\x03A8', new char[] { '\x03C8' }),                      /* 03A8; 03C8; Case map */
            new CharMap('\x03A9', new char[] { '\x03C9' }),                      /* 03A9; 03C9; Case map */
            new CharMap('\x03AA', new char[] { '\x03CA' }),                      /* 03AA; 03CA; Case map */
            new CharMap('\x03AB', new char[] { '\x03CB' }),                      /* 03AB; 03CB; Case map */
            new CharMap('\x03B0', new char[] { '\x03C5',               /* 03B0; 03C5 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x03C2', new char[] { '\x03C3' }),                      /* 03C2; 03C3; Case map */
            new CharMap('\x03D0', new char[] { '\x03B2' }),                      /* 03D0; 03B2; Case map */
            new CharMap('\x03D1', new char[] { '\x03B8' }),                      /* 03D1; 03B8; Case map */
            new CharMap('\x03D2', new char[] { '\x03C5' }),            /* 03D2; 03C5; Additional folding */
            new CharMap('\x03D3', new char[] { '\x03CD' }),            /* 03D3; 03CD; Additional folding */
            new CharMap('\x03D4', new char[] { '\x03CB' }),            /* 03D4; 03CB; Additional folding */
            new CharMap('\x03D5', new char[] { '\x03C6' }),                      /* 03D5; 03C6; Case map */
            new CharMap('\x03D6', new char[] { '\x03C0' }),                      /* 03D6; 03C0; Case map */
            new CharMap('\x03D8', new char[] { '\x03D9' }),                      /* 03D8; 03D9; Case map */
            new CharMap('\x03DA', new char[] { '\x03DB' }),                      /* 03DA; 03DB; Case map */
            new CharMap('\x03DC', new char[] { '\x03DD' }),                      /* 03DC; 03DD; Case map */
            new CharMap('\x03DE', new char[] { '\x03DF' }),                      /* 03DE; 03DF; Case map */
            new CharMap('\x03E0', new char[] { '\x03E1' }),                      /* 03E0; 03E1; Case map */
            new CharMap('\x03E2', new char[] { '\x03E3' }),                      /* 03E2; 03E3; Case map */
            new CharMap('\x03E4', new char[] { '\x03E5' }),                      /* 03E4; 03E5; Case map */
            new CharMap('\x03E6', new char[] { '\x03E7' }),                      /* 03E6; 03E7; Case map */
            new CharMap('\x03E8', new char[] { '\x03E9' }),                      /* 03E8; 03E9; Case map */
            new CharMap('\x03EA', new char[] { '\x03EB' }),                      /* 03EA; 03EB; Case map */
            new CharMap('\x03EC', new char[] { '\x03ED' }),                      /* 03EC; 03ED; Case map */
            new CharMap('\x03EE', new char[] { '\x03EF' }),                      /* 03EE; 03EF; Case map */
            new CharMap('\x03F0', new char[] { '\x03BA' }),                      /* 03F0; 03BA; Case map */
            new CharMap('\x03F1', new char[] { '\x03C1' }),                      /* 03F1; 03C1; Case map */
            new CharMap('\x03F2', new char[] { '\x03C3' }),                      /* 03F2; 03C3; Case map */
            new CharMap('\x03F4', new char[] { '\x03B8' }),                      /* 03F4; 03B8; Case map */
            new CharMap('\x03F5', new char[] { '\x03B5' }),                      /* 03F5; 03B5; Case map */
            new CharMap('\x0400', new char[] { '\x0450' }),                      /* 0400; 0450; Case map */
            new CharMap('\x0401', new char[] { '\x0451' }),                      /* 0401; 0451; Case map */
            new CharMap('\x0402', new char[] { '\x0452' }),                      /* 0402; 0452; Case map */
            new CharMap('\x0403', new char[] { '\x0453' }),                      /* 0403; 0453; Case map */
            new CharMap('\x0404', new char[] { '\x0454' }),                      /* 0404; 0454; Case map */
            new CharMap('\x0405', new char[] { '\x0455' }),                      /* 0405; 0455; Case map */
            new CharMap('\x0406', new char[] { '\x0456' }),                      /* 0406; 0456; Case map */
            new CharMap('\x0407', new char[] { '\x0457' }),                      /* 0407; 0457; Case map */
            new CharMap('\x0408', new char[] { '\x0458' }),                      /* 0408; 0458; Case map */
            new CharMap('\x0409', new char[] { '\x0459' }),                      /* 0409; 0459; Case map */
            new CharMap('\x040A', new char[] { '\x045A' }),                      /* 040A; 045A; Case map */
            new CharMap('\x040B', new char[] { '\x045B' }),                      /* 040B; 045B; Case map */
            new CharMap('\x040C', new char[] { '\x045C' }),                      /* 040C; 045C; Case map */
            new CharMap('\x040D', new char[] { '\x045D' }),                      /* 040D; 045D; Case map */
            new CharMap('\x040E', new char[] { '\x045E' }),                      /* 040E; 045E; Case map */
            new CharMap('\x040F', new char[] { '\x045F' }),                      /* 040F; 045F; Case map */
            new CharMap('\x0410', new char[] { '\x0430' }),                      /* 0410; 0430; Case map */
            new CharMap('\x0411', new char[] { '\x0431' }),                      /* 0411; 0431; Case map */
            new CharMap('\x0412', new char[] { '\x0432' }),                      /* 0412; 0432; Case map */
            new CharMap('\x0413', new char[] { '\x0433' }),                      /* 0413; 0433; Case map */
            new CharMap('\x0414', new char[] { '\x0434' }),                      /* 0414; 0434; Case map */
            new CharMap('\x0415', new char[] { '\x0435' }),                      /* 0415; 0435; Case map */
            new CharMap('\x0416', new char[] { '\x0436' }),                      /* 0416; 0436; Case map */
            new CharMap('\x0417', new char[] { '\x0437' }),                      /* 0417; 0437; Case map */
            new CharMap('\x0418', new char[] { '\x0438' }),                      /* 0418; 0438; Case map */
            new CharMap('\x0419', new char[] { '\x0439' }),                      /* 0419; 0439; Case map */
            new CharMap('\x041A', new char[] { '\x043A' }),                      /* 041A; 043A; Case map */
            new CharMap('\x041B', new char[] { '\x043B' }),                      /* 041B; 043B; Case map */
            new CharMap('\x041C', new char[] { '\x043C' }),                      /* 041C; 043C; Case map */
            new CharMap('\x041D', new char[] { '\x043D' }),                      /* 041D; 043D; Case map */
            new CharMap('\x041E', new char[] { '\x043E' }),                      /* 041E; 043E; Case map */
            new CharMap('\x041F', new char[] { '\x043F' }),                      /* 041F; 043F; Case map */
            new CharMap('\x0420', new char[] { '\x0440' }),                      /* 0420; 0440; Case map */
            new CharMap('\x0421', new char[] { '\x0441' }),                      /* 0421; 0441; Case map */
            new CharMap('\x0422', new char[] { '\x0442' }),                      /* 0422; 0442; Case map */
            new CharMap('\x0423', new char[] { '\x0443' }),                      /* 0423; 0443; Case map */
            new CharMap('\x0424', new char[] { '\x0444' }),                      /* 0424; 0444; Case map */
            new CharMap('\x0425', new char[] { '\x0445' }),                      /* 0425; 0445; Case map */
            new CharMap('\x0426', new char[] { '\x0446' }),                      /* 0426; 0446; Case map */
            new CharMap('\x0427', new char[] { '\x0447' }),                      /* 0427; 0447; Case map */
            new CharMap('\x0428', new char[] { '\x0448' }),                      /* 0428; 0448; Case map */
            new CharMap('\x0429', new char[] { '\x0449' }),                      /* 0429; 0449; Case map */
            new CharMap('\x042A', new char[] { '\x044A' }),                      /* 042A; 044A; Case map */
            new CharMap('\x042B', new char[] { '\x044B' }),                      /* 042B; 044B; Case map */
            new CharMap('\x042C', new char[] { '\x044C' }),                      /* 042C; 044C; Case map */
            new CharMap('\x042D', new char[] { '\x044D' }),                      /* 042D; 044D; Case map */
            new CharMap('\x042E', new char[] { '\x044E' }),                      /* 042E; 044E; Case map */
            new CharMap('\x042F', new char[] { '\x044F' }),                      /* 042F; 044F; Case map */
            new CharMap('\x0460', new char[] { '\x0461' }),                      /* 0460; 0461; Case map */
            new CharMap('\x0462', new char[] { '\x0463' }),                      /* 0462; 0463; Case map */
            new CharMap('\x0464', new char[] { '\x0465' }),                      /* 0464; 0465; Case map */
            new CharMap('\x0466', new char[] { '\x0467' }),                      /* 0466; 0467; Case map */
            new CharMap('\x0468', new char[] { '\x0469' }),                      /* 0468; 0469; Case map */
            new CharMap('\x046A', new char[] { '\x046B' }),                      /* 046A; 046B; Case map */
            new CharMap('\x046C', new char[] { '\x046D' }),                      /* 046C; 046D; Case map */
            new CharMap('\x046E', new char[] { '\x046F' }),                      /* 046E; 046F; Case map */
            new CharMap('\x0470', new char[] { '\x0471' }),                      /* 0470; 0471; Case map */
            new CharMap('\x0472', new char[] { '\x0473' }),                      /* 0472; 0473; Case map */
            new CharMap('\x0474', new char[] { '\x0475' }),                      /* 0474; 0475; Case map */
            new CharMap('\x0476', new char[] { '\x0477' }),                      /* 0476; 0477; Case map */
            new CharMap('\x0478', new char[] { '\x0479' }),                      /* 0478; 0479; Case map */
            new CharMap('\x047A', new char[] { '\x047B' }),                      /* 047A; 047B; Case map */
            new CharMap('\x047C', new char[] { '\x047D' }),                      /* 047C; 047D; Case map */
            new CharMap('\x047E', new char[] { '\x047F' }),                      /* 047E; 047F; Case map */
            new CharMap('\x0480', new char[] { '\x0481' }),                      /* 0480; 0481; Case map */
            new CharMap('\x048A', new char[] { '\x048B' }),                      /* 048A; 048B; Case map */
            new CharMap('\x048C', new char[] { '\x048D' }),                      /* 048C; 048D; Case map */
            new CharMap('\x048E', new char[] { '\x048F' }),                      /* 048E; 048F; Case map */
            new CharMap('\x0490', new char[] { '\x0491' }),                      /* 0490; 0491; Case map */
            new CharMap('\x0492', new char[] { '\x0493' }),                      /* 0492; 0493; Case map */
            new CharMap('\x0494', new char[] { '\x0495' }),                      /* 0494; 0495; Case map */
            new CharMap('\x0496', new char[] { '\x0497' }),                      /* 0496; 0497; Case map */
            new CharMap('\x0498', new char[] { '\x0499' }),                      /* 0498; 0499; Case map */
            new CharMap('\x049A', new char[] { '\x049B' }),                      /* 049A; 049B; Case map */
            new CharMap('\x049C', new char[] { '\x049D' }),                      /* 049C; 049D; Case map */
            new CharMap('\x049E', new char[] { '\x049F' }),                      /* 049E; 049F; Case map */
            new CharMap('\x04A0', new char[] { '\x04A1' }),                      /* 04A0; 04A1; Case map */
            new CharMap('\x04A2', new char[] { '\x04A3' }),                      /* 04A2; 04A3; Case map */
            new CharMap('\x04A4', new char[] { '\x04A5' }),                      /* 04A4; 04A5; Case map */
            new CharMap('\x04A6', new char[] { '\x04A7' }),                      /* 04A6; 04A7; Case map */
            new CharMap('\x04A8', new char[] { '\x04A9' }),                      /* 04A8; 04A9; Case map */
            new CharMap('\x04AA', new char[] { '\x04AB' }),                      /* 04AA; 04AB; Case map */
            new CharMap('\x04AC', new char[] { '\x04AD' }),                      /* 04AC; 04AD; Case map */
            new CharMap('\x04AE', new char[] { '\x04AF' }),                      /* 04AE; 04AF; Case map */
            new CharMap('\x04B0', new char[] { '\x04B1' }),                      /* 04B0; 04B1; Case map */
            new CharMap('\x04B2', new char[] { '\x04B3' }),                      /* 04B2; 04B3; Case map */
            new CharMap('\x04B4', new char[] { '\x04B5' }),                      /* 04B4; 04B5; Case map */
            new CharMap('\x04B6', new char[] { '\x04B7' }),                      /* 04B6; 04B7; Case map */
            new CharMap('\x04B8', new char[] { '\x04B9' }),                      /* 04B8; 04B9; Case map */
            new CharMap('\x04BA', new char[] { '\x04BB' }),                      /* 04BA; 04BB; Case map */
            new CharMap('\x04BC', new char[] { '\x04BD' }),                      /* 04BC; 04BD; Case map */
            new CharMap('\x04BE', new char[] { '\x04BF' }),                      /* 04BE; 04BF; Case map */
            new CharMap('\x04C1', new char[] { '\x04C2' }),                      /* 04C1; 04C2; Case map */
            new CharMap('\x04C3', new char[] { '\x04C4' }),                      /* 04C3; 04C4; Case map */
            new CharMap('\x04C5', new char[] { '\x04C6' }),                      /* 04C5; 04C6; Case map */
            new CharMap('\x04C7', new char[] { '\x04C8' }),                      /* 04C7; 04C8; Case map */
            new CharMap('\x04C9', new char[] { '\x04CA' }),                      /* 04C9; 04CA; Case map */
            new CharMap('\x04CB', new char[] { '\x04CC' }),                      /* 04CB; 04CC; Case map */
            new CharMap('\x04CD', new char[] { '\x04CE' }),                      /* 04CD; 04CE; Case map */
            new CharMap('\x04D0', new char[] { '\x04D1' }),                      /* 04D0; 04D1; Case map */
            new CharMap('\x04D2', new char[] { '\x04D3' }),                      /* 04D2; 04D3; Case map */
            new CharMap('\x04D4', new char[] { '\x04D5' }),                      /* 04D4; 04D5; Case map */
            new CharMap('\x04D6', new char[] { '\x04D7' }),                      /* 04D6; 04D7; Case map */
            new CharMap('\x04D8', new char[] { '\x04D9' }),                      /* 04D8; 04D9; Case map */
            new CharMap('\x04DA', new char[] { '\x04DB' }),                      /* 04DA; 04DB; Case map */
            new CharMap('\x04DC', new char[] { '\x04DD' }),                      /* 04DC; 04DD; Case map */
            new CharMap('\x04DE', new char[] { '\x04DF' }),                      /* 04DE; 04DF; Case map */
            new CharMap('\x04E0', new char[] { '\x04E1' }),                      /* 04E0; 04E1; Case map */
            new CharMap('\x04E2', new char[] { '\x04E3' }),                      /* 04E2; 04E3; Case map */
            new CharMap('\x04E4', new char[] { '\x04E5' }),                      /* 04E4; 04E5; Case map */
            new CharMap('\x04E6', new char[] { '\x04E7' }),                      /* 04E6; 04E7; Case map */
            new CharMap('\x04E8', new char[] { '\x04E9' }),                      /* 04E8; 04E9; Case map */
            new CharMap('\x04EA', new char[] { '\x04EB' }),                      /* 04EA; 04EB; Case map */
            new CharMap('\x04EC', new char[] { '\x04ED' }),                      /* 04EC; 04ED; Case map */
            new CharMap('\x04EE', new char[] { '\x04EF' }),                      /* 04EE; 04EF; Case map */
            new CharMap('\x04F0', new char[] { '\x04F1' }),                      /* 04F0; 04F1; Case map */
            new CharMap('\x04F2', new char[] { '\x04F3' }),                      /* 04F2; 04F3; Case map */
            new CharMap('\x04F4', new char[] { '\x04F5' }),                      /* 04F4; 04F5; Case map */
            new CharMap('\x04F8', new char[] { '\x04F9' }),                      /* 04F8; 04F9; Case map */
            new CharMap('\x0500', new char[] { '\x0501' }),                      /* 0500; 0501; Case map */
            new CharMap('\x0502', new char[] { '\x0503' }),                      /* 0502; 0503; Case map */
            new CharMap('\x0504', new char[] { '\x0505' }),                      /* 0504; 0505; Case map */
            new CharMap('\x0506', new char[] { '\x0507' }),                      /* 0506; 0507; Case map */
            new CharMap('\x0508', new char[] { '\x0509' }),                      /* 0508; 0509; Case map */
            new CharMap('\x050A', new char[] { '\x050B' }),                      /* 050A; 050B; Case map */
            new CharMap('\x050C', new char[] { '\x050D' }),                      /* 050C; 050D; Case map */
            new CharMap('\x050E', new char[] { '\x050F' }),                      /* 050E; 050F; Case map */
            new CharMap('\x0531', new char[] { '\x0561' }),                      /* 0531; 0561; Case map */
            new CharMap('\x0532', new char[] { '\x0562' }),                      /* 0532; 0562; Case map */
            new CharMap('\x0533', new char[] { '\x0563' }),                      /* 0533; 0563; Case map */
            new CharMap('\x0534', new char[] { '\x0564' }),                      /* 0534; 0564; Case map */
            new CharMap('\x0535', new char[] { '\x0565' }),                      /* 0535; 0565; Case map */
            new CharMap('\x0536', new char[] { '\x0566' }),                      /* 0536; 0566; Case map */
            new CharMap('\x0537', new char[] { '\x0567' }),                      /* 0537; 0567; Case map */
            new CharMap('\x0538', new char[] { '\x0568' }),                      /* 0538; 0568; Case map */
            new CharMap('\x0539', new char[] { '\x0569' }),                      /* 0539; 0569; Case map */
            new CharMap('\x053A', new char[] { '\x056A' }),                      /* 053A; 056A; Case map */
            new CharMap('\x053B', new char[] { '\x056B' }),                      /* 053B; 056B; Case map */
            new CharMap('\x053C', new char[] { '\x056C' }),                      /* 053C; 056C; Case map */
            new CharMap('\x053D', new char[] { '\x056D' }),                      /* 053D; 056D; Case map */
            new CharMap('\x053E', new char[] { '\x056E' }),                      /* 053E; 056E; Case map */
            new CharMap('\x053F', new char[] { '\x056F' }),                      /* 053F; 056F; Case map */
            new CharMap('\x0540', new char[] { '\x0570' }),                      /* 0540; 0570; Case map */
            new CharMap('\x0541', new char[] { '\x0571' }),                      /* 0541; 0571; Case map */
            new CharMap('\x0542', new char[] { '\x0572' }),                      /* 0542; 0572; Case map */
            new CharMap('\x0543', new char[] { '\x0573' }),                      /* 0543; 0573; Case map */
            new CharMap('\x0544', new char[] { '\x0574' }),                      /* 0544; 0574; Case map */
            new CharMap('\x0545', new char[] { '\x0575' }),                      /* 0545; 0575; Case map */
            new CharMap('\x0546', new char[] { '\x0576' }),                      /* 0546; 0576; Case map */
            new CharMap('\x0547', new char[] { '\x0577' }),                      /* 0547; 0577; Case map */
            new CharMap('\x0548', new char[] { '\x0578' }),                      /* 0548; 0578; Case map */
            new CharMap('\x0549', new char[] { '\x0579' }),                      /* 0549; 0579; Case map */
            new CharMap('\x054A', new char[] { '\x057A' }),                      /* 054A; 057A; Case map */
            new CharMap('\x054B', new char[] { '\x057B' }),                      /* 054B; 057B; Case map */
            new CharMap('\x054C', new char[] { '\x057C' }),                      /* 054C; 057C; Case map */
            new CharMap('\x054D', new char[] { '\x057D' }),                      /* 054D; 057D; Case map */
            new CharMap('\x054E', new char[] { '\x057E' }),                      /* 054E; 057E; Case map */
            new CharMap('\x054F', new char[] { '\x057F' }),                      /* 054F; 057F; Case map */
            new CharMap('\x0550', new char[] { '\x0580' }),                      /* 0550; 0580; Case map */
            new CharMap('\x0551', new char[] { '\x0581' }),                      /* 0551; 0581; Case map */
            new CharMap('\x0552', new char[] { '\x0582' }),                      /* 0552; 0582; Case map */
            new CharMap('\x0553', new char[] { '\x0583' }),                      /* 0553; 0583; Case map */
            new CharMap('\x0554', new char[] { '\x0584' }),                      /* 0554; 0584; Case map */
            new CharMap('\x0555', new char[] { '\x0585' }),                      /* 0555; 0585; Case map */
            new CharMap('\x0556', new char[] { '\x0586' }),                      /* 0556; 0586; Case map */
            new CharMap('\x0587', new char[] { '\x0565',                    /* 0587; 0565 0582; Case map */
                   '\x0582' }),
            new CharMap('\x1E00', new char[] { '\x1E01' }),                      /* 1E00; 1E01; Case map */
            new CharMap('\x1E02', new char[] { '\x1E03' }),                      /* 1E02; 1E03; Case map */
            new CharMap('\x1E04', new char[] { '\x1E05' }),                      /* 1E04; 1E05; Case map */
            new CharMap('\x1E06', new char[] { '\x1E07' }),                      /* 1E06; 1E07; Case map */
            new CharMap('\x1E08', new char[] { '\x1E09' }),                      /* 1E08; 1E09; Case map */
            new CharMap('\x1E0A', new char[] { '\x1E0B' }),                      /* 1E0A; 1E0B; Case map */
            new CharMap('\x1E0C', new char[] { '\x1E0D' }),                      /* 1E0C; 1E0D; Case map */
            new CharMap('\x1E0E', new char[] { '\x1E0F' }),                      /* 1E0E; 1E0F; Case map */
            new CharMap('\x1E10', new char[] { '\x1E11' }),                      /* 1E10; 1E11; Case map */
            new CharMap('\x1E12', new char[] { '\x1E13' }),                      /* 1E12; 1E13; Case map */
            new CharMap('\x1E14', new char[] { '\x1E15' }),                      /* 1E14; 1E15; Case map */
            new CharMap('\x1E16', new char[] { '\x1E17' }),                      /* 1E16; 1E17; Case map */
            new CharMap('\x1E18', new char[] { '\x1E19' }),                      /* 1E18; 1E19; Case map */
            new CharMap('\x1E1A', new char[] { '\x1E1B' }),                      /* 1E1A; 1E1B; Case map */
            new CharMap('\x1E1C', new char[] { '\x1E1D' }),                      /* 1E1C; 1E1D; Case map */
            new CharMap('\x1E1E', new char[] { '\x1E1F' }),                      /* 1E1E; 1E1F; Case map */
            new CharMap('\x1E20', new char[] { '\x1E21' }),                      /* 1E20; 1E21; Case map */
            new CharMap('\x1E22', new char[] { '\x1E23' }),                      /* 1E22; 1E23; Case map */
            new CharMap('\x1E24', new char[] { '\x1E25' }),                      /* 1E24; 1E25; Case map */
            new CharMap('\x1E26', new char[] { '\x1E27' }),                      /* 1E26; 1E27; Case map */
            new CharMap('\x1E28', new char[] { '\x1E29' }),                      /* 1E28; 1E29; Case map */
            new CharMap('\x1E2A', new char[] { '\x1E2B' }),                      /* 1E2A; 1E2B; Case map */
            new CharMap('\x1E2C', new char[] { '\x1E2D' }),                      /* 1E2C; 1E2D; Case map */
            new CharMap('\x1E2E', new char[] { '\x1E2F' }),                      /* 1E2E; 1E2F; Case map */
            new CharMap('\x1E30', new char[] { '\x1E31' }),                      /* 1E30; 1E31; Case map */
            new CharMap('\x1E32', new char[] { '\x1E33' }),                      /* 1E32; 1E33; Case map */
            new CharMap('\x1E34', new char[] { '\x1E35' }),                      /* 1E34; 1E35; Case map */
            new CharMap('\x1E36', new char[] { '\x1E37' }),                      /* 1E36; 1E37; Case map */
            new CharMap('\x1E38', new char[] { '\x1E39' }),                      /* 1E38; 1E39; Case map */
            new CharMap('\x1E3A', new char[] { '\x1E3B' }),                      /* 1E3A; 1E3B; Case map */
            new CharMap('\x1E3C', new char[] { '\x1E3D' }),                      /* 1E3C; 1E3D; Case map */
            new CharMap('\x1E3E', new char[] { '\x1E3F' }),                      /* 1E3E; 1E3F; Case map */
            new CharMap('\x1E40', new char[] { '\x1E41' }),                      /* 1E40; 1E41; Case map */
            new CharMap('\x1E42', new char[] { '\x1E43' }),                      /* 1E42; 1E43; Case map */
            new CharMap('\x1E44', new char[] { '\x1E45' }),                      /* 1E44; 1E45; Case map */
            new CharMap('\x1E46', new char[] { '\x1E47' }),                      /* 1E46; 1E47; Case map */
            new CharMap('\x1E48', new char[] { '\x1E49' }),                      /* 1E48; 1E49; Case map */
            new CharMap('\x1E4A', new char[] { '\x1E4B' }),                      /* 1E4A; 1E4B; Case map */
            new CharMap('\x1E4C', new char[] { '\x1E4D' }),                      /* 1E4C; 1E4D; Case map */
            new CharMap('\x1E4E', new char[] { '\x1E4F' }),                      /* 1E4E; 1E4F; Case map */
            new CharMap('\x1E50', new char[] { '\x1E51' }),                      /* 1E50; 1E51; Case map */
            new CharMap('\x1E52', new char[] { '\x1E53' }),                      /* 1E52; 1E53; Case map */
            new CharMap('\x1E54', new char[] { '\x1E55' }),                      /* 1E54; 1E55; Case map */
            new CharMap('\x1E56', new char[] { '\x1E57' }),                      /* 1E56; 1E57; Case map */
            new CharMap('\x1E58', new char[] { '\x1E59' }),                      /* 1E58; 1E59; Case map */
            new CharMap('\x1E5A', new char[] { '\x1E5B' }),                      /* 1E5A; 1E5B; Case map */
            new CharMap('\x1E5C', new char[] { '\x1E5D' }),                      /* 1E5C; 1E5D; Case map */
            new CharMap('\x1E5E', new char[] { '\x1E5F' }),                      /* 1E5E; 1E5F; Case map */
            new CharMap('\x1E60', new char[] { '\x1E61' }),                      /* 1E60; 1E61; Case map */
            new CharMap('\x1E62', new char[] { '\x1E63' }),                      /* 1E62; 1E63; Case map */
            new CharMap('\x1E64', new char[] { '\x1E65' }),                      /* 1E64; 1E65; Case map */
            new CharMap('\x1E66', new char[] { '\x1E67' }),                      /* 1E66; 1E67; Case map */
            new CharMap('\x1E68', new char[] { '\x1E69' }),                      /* 1E68; 1E69; Case map */
            new CharMap('\x1E6A', new char[] { '\x1E6B' }),                      /* 1E6A; 1E6B; Case map */
            new CharMap('\x1E6C', new char[] { '\x1E6D' }),                      /* 1E6C; 1E6D; Case map */
            new CharMap('\x1E6E', new char[] { '\x1E6F' }),                      /* 1E6E; 1E6F; Case map */
            new CharMap('\x1E70', new char[] { '\x1E71' }),                      /* 1E70; 1E71; Case map */
            new CharMap('\x1E72', new char[] { '\x1E73' }),                      /* 1E72; 1E73; Case map */
            new CharMap('\x1E74', new char[] { '\x1E75' }),                      /* 1E74; 1E75; Case map */
            new CharMap('\x1E76', new char[] { '\x1E77' }),                      /* 1E76; 1E77; Case map */
            new CharMap('\x1E78', new char[] { '\x1E79' }),                      /* 1E78; 1E79; Case map */
            new CharMap('\x1E7A', new char[] { '\x1E7B' }),                      /* 1E7A; 1E7B; Case map */
            new CharMap('\x1E7C', new char[] { '\x1E7D' }),                      /* 1E7C; 1E7D; Case map */
            new CharMap('\x1E7E', new char[] { '\x1E7F' }),                      /* 1E7E; 1E7F; Case map */
            new CharMap('\x1E80', new char[] { '\x1E81' }),                      /* 1E80; 1E81; Case map */
            new CharMap('\x1E82', new char[] { '\x1E83' }),                      /* 1E82; 1E83; Case map */
            new CharMap('\x1E84', new char[] { '\x1E85' }),                      /* 1E84; 1E85; Case map */
            new CharMap('\x1E86', new char[] { '\x1E87' }),                      /* 1E86; 1E87; Case map */
            new CharMap('\x1E88', new char[] { '\x1E89' }),                      /* 1E88; 1E89; Case map */
            new CharMap('\x1E8A', new char[] { '\x1E8B' }),                      /* 1E8A; 1E8B; Case map */
            new CharMap('\x1E8C', new char[] { '\x1E8D' }),                      /* 1E8C; 1E8D; Case map */
            new CharMap('\x1E8E', new char[] { '\x1E8F' }),                      /* 1E8E; 1E8F; Case map */
            new CharMap('\x1E90', new char[] { '\x1E91' }),                      /* 1E90; 1E91; Case map */
            new CharMap('\x1E92', new char[] { '\x1E93' }),                      /* 1E92; 1E93; Case map */
            new CharMap('\x1E94', new char[] { '\x1E95' }),                      /* 1E94; 1E95; Case map */
            new CharMap('\x1E96', new char[] { '\x0068',                    /* 1E96; 0068 0331; Case map */
                   '\x0331' }),
            new CharMap('\x1E97', new char[] { '\x0074',                    /* 1E97; 0074 0308; Case map */
                   '\x0308' }),
            new CharMap('\x1E98', new char[] { '\x0077',                    /* 1E98; 0077 030A; Case map */
                   '\x030A' }),
            new CharMap('\x1E99', new char[] { '\x0079',                    /* 1E99; 0079 030A; Case map */
                   '\x030A' }),
            new CharMap('\x1E9A', new char[] { '\x0061',                    /* 1E9A; 0061 02BE; Case map */
                   '\x02BE' }),
            new CharMap('\x1E9B', new char[] { '\x1E61' }),                      /* 1E9B; 1E61; Case map */
            new CharMap('\x1EA0', new char[] { '\x1EA1' }),                      /* 1EA0; 1EA1; Case map */
            new CharMap('\x1EA2', new char[] { '\x1EA3' }),                      /* 1EA2; 1EA3; Case map */
            new CharMap('\x1EA4', new char[] { '\x1EA5' }),                      /* 1EA4; 1EA5; Case map */
            new CharMap('\x1EA6', new char[] { '\x1EA7' }),                      /* 1EA6; 1EA7; Case map */
            new CharMap('\x1EA8', new char[] { '\x1EA9' }),                      /* 1EA8; 1EA9; Case map */
            new CharMap('\x1EAA', new char[] { '\x1EAB' }),                      /* 1EAA; 1EAB; Case map */
            new CharMap('\x1EAC', new char[] { '\x1EAD' }),                      /* 1EAC; 1EAD; Case map */
            new CharMap('\x1EAE', new char[] { '\x1EAF' }),                      /* 1EAE; 1EAF; Case map */
            new CharMap('\x1EB0', new char[] { '\x1EB1' }),                      /* 1EB0; 1EB1; Case map */
            new CharMap('\x1EB2', new char[] { '\x1EB3' }),                      /* 1EB2; 1EB3; Case map */
            new CharMap('\x1EB4', new char[] { '\x1EB5' }),                      /* 1EB4; 1EB5; Case map */
            new CharMap('\x1EB6', new char[] { '\x1EB7' }),                      /* 1EB6; 1EB7; Case map */
            new CharMap('\x1EB8', new char[] { '\x1EB9' }),                      /* 1EB8; 1EB9; Case map */
            new CharMap('\x1EBA', new char[] { '\x1EBB' }),                      /* 1EBA; 1EBB; Case map */
            new CharMap('\x1EBC', new char[] { '\x1EBD' }),                      /* 1EBC; 1EBD; Case map */
            new CharMap('\x1EBE', new char[] { '\x1EBF' }),                      /* 1EBE; 1EBF; Case map */
            new CharMap('\x1EC0', new char[] { '\x1EC1' }),                      /* 1EC0; 1EC1; Case map */
            new CharMap('\x1EC2', new char[] { '\x1EC3' }),                      /* 1EC2; 1EC3; Case map */
            new CharMap('\x1EC4', new char[] { '\x1EC5' }),                      /* 1EC4; 1EC5; Case map */
            new CharMap('\x1EC6', new char[] { '\x1EC7' }),                      /* 1EC6; 1EC7; Case map */
            new CharMap('\x1EC8', new char[] { '\x1EC9' }),                      /* 1EC8; 1EC9; Case map */
            new CharMap('\x1ECA', new char[] { '\x1ECB' }),                      /* 1ECA; 1ECB; Case map */
            new CharMap('\x1ECC', new char[] { '\x1ECD' }),                      /* 1ECC; 1ECD; Case map */
            new CharMap('\x1ECE', new char[] { '\x1ECF' }),                      /* 1ECE; 1ECF; Case map */
            new CharMap('\x1ED0', new char[] { '\x1ED1' }),                      /* 1ED0; 1ED1; Case map */
            new CharMap('\x1ED2', new char[] { '\x1ED3' }),                      /* 1ED2; 1ED3; Case map */
            new CharMap('\x1ED4', new char[] { '\x1ED5' }),                      /* 1ED4; 1ED5; Case map */
            new CharMap('\x1ED6', new char[] { '\x1ED7' }),                      /* 1ED6; 1ED7; Case map */
            new CharMap('\x1ED8', new char[] { '\x1ED9' }),                      /* 1ED8; 1ED9; Case map */
            new CharMap('\x1EDA', new char[] { '\x1EDB' }),                      /* 1EDA; 1EDB; Case map */
            new CharMap('\x1EDC', new char[] { '\x1EDD' }),                      /* 1EDC; 1EDD; Case map */
            new CharMap('\x1EDE', new char[] { '\x1EDF' }),                      /* 1EDE; 1EDF; Case map */
            new CharMap('\x1EE0', new char[] { '\x1EE1' }),                      /* 1EE0; 1EE1; Case map */
            new CharMap('\x1EE2', new char[] { '\x1EE3' }),                      /* 1EE2; 1EE3; Case map */
            new CharMap('\x1EE4', new char[] { '\x1EE5' }),                      /* 1EE4; 1EE5; Case map */
            new CharMap('\x1EE6', new char[] { '\x1EE7' }),                      /* 1EE6; 1EE7; Case map */
            new CharMap('\x1EE8', new char[] { '\x1EE9' }),                      /* 1EE8; 1EE9; Case map */
            new CharMap('\x1EEA', new char[] { '\x1EEB' }),                      /* 1EEA; 1EEB; Case map */
            new CharMap('\x1EEC', new char[] { '\x1EED' }),                      /* 1EEC; 1EED; Case map */
            new CharMap('\x1EEE', new char[] { '\x1EEF' }),                      /* 1EEE; 1EEF; Case map */
            new CharMap('\x1EF0', new char[] { '\x1EF1' }),                      /* 1EF0; 1EF1; Case map */
            new CharMap('\x1EF2', new char[] { '\x1EF3' }),                      /* 1EF2; 1EF3; Case map */
            new CharMap('\x1EF4', new char[] { '\x1EF5' }),                      /* 1EF4; 1EF5; Case map */
            new CharMap('\x1EF6', new char[] { '\x1EF7' }),                      /* 1EF6; 1EF7; Case map */
            new CharMap('\x1EF8', new char[] { '\x1EF9' }),                      /* 1EF8; 1EF9; Case map */
            new CharMap('\x1F08', new char[] { '\x1F00' }),                      /* 1F08; 1F00; Case map */
            new CharMap('\x1F09', new char[] { '\x1F01' }),                      /* 1F09; 1F01; Case map */
            new CharMap('\x1F0A', new char[] { '\x1F02' }),                      /* 1F0A; 1F02; Case map */
            new CharMap('\x1F0B', new char[] { '\x1F03' }),                      /* 1F0B; 1F03; Case map */
            new CharMap('\x1F0C', new char[] { '\x1F04' }),                      /* 1F0C; 1F04; Case map */
            new CharMap('\x1F0D', new char[] { '\x1F05' }),                      /* 1F0D; 1F05; Case map */
            new CharMap('\x1F0E', new char[] { '\x1F06' }),                      /* 1F0E; 1F06; Case map */
            new CharMap('\x1F0F', new char[] { '\x1F07' }),                      /* 1F0F; 1F07; Case map */
            new CharMap('\x1F18', new char[] { '\x1F10' }),                      /* 1F18; 1F10; Case map */
            new CharMap('\x1F19', new char[] { '\x1F11' }),                      /* 1F19; 1F11; Case map */
            new CharMap('\x1F1A', new char[] { '\x1F12' }),                      /* 1F1A; 1F12; Case map */
            new CharMap('\x1F1B', new char[] { '\x1F13' }),                      /* 1F1B; 1F13; Case map */
            new CharMap('\x1F1C', new char[] { '\x1F14' }),                      /* 1F1C; 1F14; Case map */
            new CharMap('\x1F1D', new char[] { '\x1F15' }),                      /* 1F1D; 1F15; Case map */
            new CharMap('\x1F28', new char[] { '\x1F20' }),                      /* 1F28; 1F20; Case map */
            new CharMap('\x1F29', new char[] { '\x1F21' }),                      /* 1F29; 1F21; Case map */
            new CharMap('\x1F2A', new char[] { '\x1F22' }),                      /* 1F2A; 1F22; Case map */
            new CharMap('\x1F2B', new char[] { '\x1F23' }),                      /* 1F2B; 1F23; Case map */
            new CharMap('\x1F2C', new char[] { '\x1F24' }),                      /* 1F2C; 1F24; Case map */
            new CharMap('\x1F2D', new char[] { '\x1F25' }),                      /* 1F2D; 1F25; Case map */
            new CharMap('\x1F2E', new char[] { '\x1F26' }),                      /* 1F2E; 1F26; Case map */
            new CharMap('\x1F2F', new char[] { '\x1F27' }),                      /* 1F2F; 1F27; Case map */
            new CharMap('\x1F38', new char[] { '\x1F30' }),                      /* 1F38; 1F30; Case map */
            new CharMap('\x1F39', new char[] { '\x1F31' }),                      /* 1F39; 1F31; Case map */
            new CharMap('\x1F3A', new char[] { '\x1F32' }),                      /* 1F3A; 1F32; Case map */
            new CharMap('\x1F3B', new char[] { '\x1F33' }),                      /* 1F3B; 1F33; Case map */
            new CharMap('\x1F3C', new char[] { '\x1F34' }),                      /* 1F3C; 1F34; Case map */
            new CharMap('\x1F3D', new char[] { '\x1F35' }),                      /* 1F3D; 1F35; Case map */
            new CharMap('\x1F3E', new char[] { '\x1F36' }),                      /* 1F3E; 1F36; Case map */
            new CharMap('\x1F3F', new char[] { '\x1F37' }),                      /* 1F3F; 1F37; Case map */
            new CharMap('\x1F48', new char[] { '\x1F40' }),                      /* 1F48; 1F40; Case map */
            new CharMap('\x1F49', new char[] { '\x1F41' }),                      /* 1F49; 1F41; Case map */
            new CharMap('\x1F4A', new char[] { '\x1F42' }),                      /* 1F4A; 1F42; Case map */
            new CharMap('\x1F4B', new char[] { '\x1F43' }),                      /* 1F4B; 1F43; Case map */
            new CharMap('\x1F4C', new char[] { '\x1F44' }),                      /* 1F4C; 1F44; Case map */
            new CharMap('\x1F4D', new char[] { '\x1F45' }),                      /* 1F4D; 1F45; Case map */
            new CharMap('\x1F50', new char[] { '\x03C5',                    /* 1F50; 03C5 0313; Case map */
                   '\x0313' }),
            new CharMap('\x1F52', new char[] { '\x03C5',               /* 1F52; 03C5 0313 0300; Case map */
                   '\x0313', '\x0300' }),
            new CharMap('\x1F54', new char[] { '\x03C5',               /* 1F54; 03C5 0313 0301; Case map */
                   '\x0313', '\x0301' }),
            new CharMap('\x1F56', new char[] { '\x03C5',               /* 1F56; 03C5 0313 0342; Case map */
                   '\x0313', '\x0342' }),
            new CharMap('\x1F59', new char[] { '\x1F51' }),                      /* 1F59; 1F51; Case map */
            new CharMap('\x1F5B', new char[] { '\x1F53' }),                      /* 1F5B; 1F53; Case map */
            new CharMap('\x1F5D', new char[] { '\x1F55' }),                      /* 1F5D; 1F55; Case map */
            new CharMap('\x1F5F', new char[] { '\x1F57' }),                      /* 1F5F; 1F57; Case map */
            new CharMap('\x1F68', new char[] { '\x1F60' }),                      /* 1F68; 1F60; Case map */
            new CharMap('\x1F69', new char[] { '\x1F61' }),                      /* 1F69; 1F61; Case map */
            new CharMap('\x1F6A', new char[] { '\x1F62' }),                      /* 1F6A; 1F62; Case map */
            new CharMap('\x1F6B', new char[] { '\x1F63' }),                      /* 1F6B; 1F63; Case map */
            new CharMap('\x1F6C', new char[] { '\x1F64' }),                      /* 1F6C; 1F64; Case map */
            new CharMap('\x1F6D', new char[] { '\x1F65' }),                      /* 1F6D; 1F65; Case map */
            new CharMap('\x1F6E', new char[] { '\x1F66' }),                      /* 1F6E; 1F66; Case map */
            new CharMap('\x1F6F', new char[] { '\x1F67' }),                      /* 1F6F; 1F67; Case map */
            new CharMap('\x1F80', new char[] { '\x1F00',                    /* 1F80; 1F00 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F81', new char[] { '\x1F01',                    /* 1F81; 1F01 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F82', new char[] { '\x1F02',                    /* 1F82; 1F02 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F83', new char[] { '\x1F03',                    /* 1F83; 1F03 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F84', new char[] { '\x1F04',                    /* 1F84; 1F04 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F85', new char[] { '\x1F05',                    /* 1F85; 1F05 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F86', new char[] { '\x1F06',                    /* 1F86; 1F06 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F87', new char[] { '\x1F07',                    /* 1F87; 1F07 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F88', new char[] { '\x1F00',                    /* 1F88; 1F00 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F89', new char[] { '\x1F01',                    /* 1F89; 1F01 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8A', new char[] { '\x1F02',                    /* 1F8A; 1F02 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8B', new char[] { '\x1F03',                    /* 1F8B; 1F03 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8C', new char[] { '\x1F04',                    /* 1F8C; 1F04 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8D', new char[] { '\x1F05',                    /* 1F8D; 1F05 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8E', new char[] { '\x1F06',                    /* 1F8E; 1F06 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8F', new char[] { '\x1F07',                    /* 1F8F; 1F07 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F90', new char[] { '\x1F20',                    /* 1F90; 1F20 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F91', new char[] { '\x1F21',                    /* 1F91; 1F21 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F92', new char[] { '\x1F22',                    /* 1F92; 1F22 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F93', new char[] { '\x1F23',                    /* 1F93; 1F23 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F94', new char[] { '\x1F24',                    /* 1F94; 1F24 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F95', new char[] { '\x1F25',                    /* 1F95; 1F25 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F96', new char[] { '\x1F26',                    /* 1F96; 1F26 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F97', new char[] { '\x1F27',                    /* 1F97; 1F27 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F98', new char[] { '\x1F20',                    /* 1F98; 1F20 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F99', new char[] { '\x1F21',                    /* 1F99; 1F21 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9A', new char[] { '\x1F22',                    /* 1F9A; 1F22 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9B', new char[] { '\x1F23',                    /* 1F9B; 1F23 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9C', new char[] { '\x1F24',                    /* 1F9C; 1F24 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9D', new char[] { '\x1F25',                    /* 1F9D; 1F25 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9E', new char[] { '\x1F26',                    /* 1F9E; 1F26 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9F', new char[] { '\x1F27',                    /* 1F9F; 1F27 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA0', new char[] { '\x1F60',                    /* 1FA0; 1F60 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA1', new char[] { '\x1F61',                    /* 1FA1; 1F61 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA2', new char[] { '\x1F62',                    /* 1FA2; 1F62 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA3', new char[] { '\x1F63',                    /* 1FA3; 1F63 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA4', new char[] { '\x1F64',                    /* 1FA4; 1F64 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA5', new char[] { '\x1F65',                    /* 1FA5; 1F65 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA6', new char[] { '\x1F66',                    /* 1FA6; 1F66 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA7', new char[] { '\x1F67',                    /* 1FA7; 1F67 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA8', new char[] { '\x1F60',                    /* 1FA8; 1F60 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA9', new char[] { '\x1F61',                    /* 1FA9; 1F61 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAA', new char[] { '\x1F62',                    /* 1FAA; 1F62 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAB', new char[] { '\x1F63',                    /* 1FAB; 1F63 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAC', new char[] { '\x1F64',                    /* 1FAC; 1F64 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAD', new char[] { '\x1F65',                    /* 1FAD; 1F65 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAE', new char[] { '\x1F66',                    /* 1FAE; 1F66 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAF', new char[] { '\x1F67',                    /* 1FAF; 1F67 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB2', new char[] { '\x1F70',                    /* 1FB2; 1F70 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB3', new char[] { '\x03B1',                    /* 1FB3; 03B1 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB4', new char[] { '\x03AC',                    /* 1FB4; 03AC 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB6', new char[] { '\x03B1',                    /* 1FB6; 03B1 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FB7', new char[] { '\x03B1',               /* 1FB7; 03B1 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FB8', new char[] { '\x1FB0' }),                      /* 1FB8; 1FB0; Case map */
            new CharMap('\x1FB9', new char[] { '\x1FB1' }),                      /* 1FB9; 1FB1; Case map */
            new CharMap('\x1FBA', new char[] { '\x1F70' }),                      /* 1FBA; 1F70; Case map */
            new CharMap('\x1FBB', new char[] { '\x1F71' }),                      /* 1FBB; 1F71; Case map */
            new CharMap('\x1FBC', new char[] { '\x03B1',                    /* 1FBC; 03B1 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FBE', new char[] { '\x03B9' }),                      /* 1FBE; 03B9; Case map */
            new CharMap('\x1FC2', new char[] { '\x1F74',                    /* 1FC2; 1F74 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC3', new char[] { '\x03B7',                    /* 1FC3; 03B7 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC4', new char[] { '\x03AE',                    /* 1FC4; 03AE 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC6', new char[] { '\x03B7',                    /* 1FC6; 03B7 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FC7', new char[] { '\x03B7',               /* 1FC7; 03B7 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FC8', new char[] { '\x1F72' }),                      /* 1FC8; 1F72; Case map */
            new CharMap('\x1FC9', new char[] { '\x1F73' }),                      /* 1FC9; 1F73; Case map */
            new CharMap('\x1FCA', new char[] { '\x1F74' }),                      /* 1FCA; 1F74; Case map */
            new CharMap('\x1FCB', new char[] { '\x1F75' }),                      /* 1FCB; 1F75; Case map */
            new CharMap('\x1FCC', new char[] { '\x03B7',                    /* 1FCC; 03B7 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FD2', new char[] { '\x03B9',               /* 1FD2; 03B9 0308 0300; Case map */
                   '\x0308', '\x0300' }),
            new CharMap('\x1FD3', new char[] { '\x03B9',               /* 1FD3; 03B9 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x1FD6', new char[] { '\x03B9',                    /* 1FD6; 03B9 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FD7', new char[] { '\x03B9',               /* 1FD7; 03B9 0308 0342; Case map */
                   '\x0308', '\x0342' }),
            new CharMap('\x1FD8', new char[] { '\x1FD0' }),                      /* 1FD8; 1FD0; Case map */
            new CharMap('\x1FD9', new char[] { '\x1FD1' }),                      /* 1FD9; 1FD1; Case map */
            new CharMap('\x1FDA', new char[] { '\x1F76' }),                      /* 1FDA; 1F76; Case map */
            new CharMap('\x1FDB', new char[] { '\x1F77' }),                      /* 1FDB; 1F77; Case map */
            new CharMap('\x1FE2', new char[] { '\x03C5',               /* 1FE2; 03C5 0308 0300; Case map */
                   '\x0308', '\x0300' }),
            new CharMap('\x1FE3', new char[] { '\x03C5',               /* 1FE3; 03C5 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x1FE4', new char[] { '\x03C1',                    /* 1FE4; 03C1 0313; Case map */
                   '\x0313' }),
            new CharMap('\x1FE6', new char[] { '\x03C5',                    /* 1FE6; 03C5 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FE7', new char[] { '\x03C5',               /* 1FE7; 03C5 0308 0342; Case map */
                   '\x0308', '\x0342' }),
            new CharMap('\x1FE8', new char[] { '\x1FE0' }),                      /* 1FE8; 1FE0; Case map */
            new CharMap('\x1FE9', new char[] { '\x1FE1' }),                      /* 1FE9; 1FE1; Case map */
            new CharMap('\x1FEA', new char[] { '\x1F7A' }),                      /* 1FEA; 1F7A; Case map */
            new CharMap('\x1FEB', new char[] { '\x1F7B' }),                      /* 1FEB; 1F7B; Case map */
            new CharMap('\x1FEC', new char[] { '\x1FE5' }),                      /* 1FEC; 1FE5; Case map */
            new CharMap('\x1FF2', new char[] { '\x1F7C',                    /* 1FF2; 1F7C 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF3', new char[] { '\x03C9',                    /* 1FF3; 03C9 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF4', new char[] { '\x03CE',                    /* 1FF4; 03CE 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF6', new char[] { '\x03C9',                    /* 1FF6; 03C9 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FF7', new char[] { '\x03C9',               /* 1FF7; 03C9 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FF8', new char[] { '\x1F78' }),                      /* 1FF8; 1F78; Case map */
            new CharMap('\x1FF9', new char[] { '\x1F79' }),                      /* 1FF9; 1F79; Case map */
            new CharMap('\x1FFA', new char[] { '\x1F7C' }),                      /* 1FFA; 1F7C; Case map */
            new CharMap('\x1FFB', new char[] { '\x1F7D' }),                      /* 1FFB; 1F7D; Case map */
            new CharMap('\x1FFC', new char[] { '\x03C9',                    /* 1FFC; 03C9 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x20A8', new char[] { '\x0072',          /* 20A8; 0072 0073; Additional folding */
                   '\x0073' }),
            new CharMap('\x2102', new char[] { '\x0063' }),            /* 2102; 0063; Additional folding */
            new CharMap('\x2103', new char[] { '\x00B0',          /* 2103; 00B0 0063; Additional folding */
                   '\x0063' }),
            new CharMap('\x2107', new char[] { '\x025B' }),            /* 2107; 025B; Additional folding */
            new CharMap('\x2109', new char[] { '\x00B0',          /* 2109; 00B0 0066; Additional folding */
                   '\x0066' }),
            new CharMap('\x210B', new char[] { '\x0068' }),            /* 210B; 0068; Additional folding */
            new CharMap('\x210C', new char[] { '\x0068' }),            /* 210C; 0068; Additional folding */
            new CharMap('\x210D', new char[] { '\x0068' }),            /* 210D; 0068; Additional folding */
            new CharMap('\x2110', new char[] { '\x0069' }),            /* 2110; 0069; Additional folding */
            new CharMap('\x2111', new char[] { '\x0069' }),            /* 2111; 0069; Additional folding */
            new CharMap('\x2112', new char[] { '\x006C' }),            /* 2112; 006C; Additional folding */
            new CharMap('\x2115', new char[] { '\x006E' }),            /* 2115; 006E; Additional folding */
            new CharMap('\x2116', new char[] { '\x006E',          /* 2116; 006E 006F; Additional folding */
                   '\x006F' }),
            new CharMap('\x2119', new char[] { '\x0070' }),            /* 2119; 0070; Additional folding */
            new CharMap('\x211A', new char[] { '\x0071' }),            /* 211A; 0071; Additional folding */
            new CharMap('\x211B', new char[] { '\x0072' }),            /* 211B; 0072; Additional folding */
            new CharMap('\x211C', new char[] { '\x0072' }),            /* 211C; 0072; Additional folding */
            new CharMap('\x211D', new char[] { '\x0072' }),            /* 211D; 0072; Additional folding */
            new CharMap('\x2120', new char[] { '\x0073',          /* 2120; 0073 006D; Additional folding */
                   '\x006D' }),
            new CharMap('\x2121', new char[] { '\x0074',     /* 2121; 0074 0065 006C; Additional folding */
                   '\x0065', '\x006C' }),
            new CharMap('\x2122', new char[] { '\x0074',          /* 2122; 0074 006D; Additional folding */
                   '\x006D' }),
            new CharMap('\x2124', new char[] { '\x007A' }),            /* 2124; 007A; Additional folding */
            new CharMap('\x2126', new char[] { '\x03C9' }),                      /* 2126; 03C9; Case map */
            new CharMap('\x2128', new char[] { '\x007A' }),            /* 2128; 007A; Additional folding */
            new CharMap('\x212A', new char[] { '\x006B' }),                      /* 212A; 006B; Case map */
            new CharMap('\x212B', new char[] { '\x00E5' }),                      /* 212B; 00E5; Case map */
            new CharMap('\x212C', new char[] { '\x0062' }),            /* 212C; 0062; Additional folding */
            new CharMap('\x212D', new char[] { '\x0063' }),            /* 212D; 0063; Additional folding */
            new CharMap('\x2130', new char[] { '\x0065' }),            /* 2130; 0065; Additional folding */
            new CharMap('\x2131', new char[] { '\x0066' }),            /* 2131; 0066; Additional folding */
            new CharMap('\x2133', new char[] { '\x006D' }),            /* 2133; 006D; Additional folding */
            new CharMap('\x213E', new char[] { '\x03B3' }),            /* 213E; 03B3; Additional folding */
            new CharMap('\x213F', new char[] { '\x03C0' }),            /* 213F; 03C0; Additional folding */
            new CharMap('\x2145', new char[] { '\x0064' }),            /* 2145; 0064; Additional folding */
            new CharMap('\x2160', new char[] { '\x2170' }),                      /* 2160; 2170; Case map */
            new CharMap('\x2161', new char[] { '\x2171' }),                      /* 2161; 2171; Case map */
            new CharMap('\x2162', new char[] { '\x2172' }),                      /* 2162; 2172; Case map */
            new CharMap('\x2163', new char[] { '\x2173' }),                      /* 2163; 2173; Case map */
            new CharMap('\x2164', new char[] { '\x2174' }),                      /* 2164; 2174; Case map */
            new CharMap('\x2165', new char[] { '\x2175' }),                      /* 2165; 2175; Case map */
            new CharMap('\x2166', new char[] { '\x2176' }),                      /* 2166; 2176; Case map */
            new CharMap('\x2167', new char[] { '\x2177' }),                      /* 2167; 2177; Case map */
            new CharMap('\x2168', new char[] { '\x2178' }),                      /* 2168; 2178; Case map */
            new CharMap('\x2169', new char[] { '\x2179' }),                      /* 2169; 2179; Case map */
            new CharMap('\x216A', new char[] { '\x217A' }),                      /* 216A; 217A; Case map */
            new CharMap('\x216B', new char[] { '\x217B' }),                      /* 216B; 217B; Case map */
            new CharMap('\x216C', new char[] { '\x217C' }),                      /* 216C; 217C; Case map */
            new CharMap('\x216D', new char[] { '\x217D' }),                      /* 216D; 217D; Case map */
            new CharMap('\x216E', new char[] { '\x217E' }),                      /* 216E; 217E; Case map */
            new CharMap('\x216F', new char[] { '\x217F' }),                      /* 216F; 217F; Case map */
            new CharMap('\x24B6', new char[] { '\x24D0' }),                      /* 24B6; 24D0; Case map */
            new CharMap('\x24B7', new char[] { '\x24D1' }),                      /* 24B7; 24D1; Case map */
            new CharMap('\x24B8', new char[] { '\x24D2' }),                      /* 24B8; 24D2; Case map */
            new CharMap('\x24B9', new char[] { '\x24D3' }),                      /* 24B9; 24D3; Case map */
            new CharMap('\x24BA', new char[] { '\x24D4' }),                      /* 24BA; 24D4; Case map */
            new CharMap('\x24BB', new char[] { '\x24D5' }),                      /* 24BB; 24D5; Case map */
            new CharMap('\x24BC', new char[] { '\x24D6' }),                      /* 24BC; 24D6; Case map */
            new CharMap('\x24BD', new char[] { '\x24D7' }),                      /* 24BD; 24D7; Case map */
            new CharMap('\x24BE', new char[] { '\x24D8' }),                      /* 24BE; 24D8; Case map */
            new CharMap('\x24BF', new char[] { '\x24D9' }),                      /* 24BF; 24D9; Case map */
            new CharMap('\x24C0', new char[] { '\x24DA' }),                      /* 24C0; 24DA; Case map */
            new CharMap('\x24C1', new char[] { '\x24DB' }),                      /* 24C1; 24DB; Case map */
            new CharMap('\x24C2', new char[] { '\x24DC' }),                      /* 24C2; 24DC; Case map */
            new CharMap('\x24C3', new char[] { '\x24DD' }),                      /* 24C3; 24DD; Case map */
            new CharMap('\x24C4', new char[] { '\x24DE' }),                      /* 24C4; 24DE; Case map */
            new CharMap('\x24C5', new char[] { '\x24DF' }),                      /* 24C5; 24DF; Case map */
            new CharMap('\x24C6', new char[] { '\x24E0' }),                      /* 24C6; 24E0; Case map */
            new CharMap('\x24C7', new char[] { '\x24E1' }),                      /* 24C7; 24E1; Case map */
            new CharMap('\x24C8', new char[] { '\x24E2' }),                      /* 24C8; 24E2; Case map */
            new CharMap('\x24C9', new char[] { '\x24E3' }),                      /* 24C9; 24E3; Case map */
            new CharMap('\x24CA', new char[] { '\x24E4' }),                      /* 24CA; 24E4; Case map */
            new CharMap('\x24CB', new char[] { '\x24E5' }),                      /* 24CB; 24E5; Case map */
            new CharMap('\x24CC', new char[] { '\x24E6' }),                      /* 24CC; 24E6; Case map */
            new CharMap('\x24CD', new char[] { '\x24E7' }),                      /* 24CD; 24E7; Case map */
            new CharMap('\x24CE', new char[] { '\x24E8' }),                      /* 24CE; 24E8; Case map */
            new CharMap('\x24CF', new char[] { '\x24E9' }),                      /* 24CF; 24E9; Case map */
            new CharMap('\x3371', new char[] { '\x0068',     /* 3371; 0068 0070 0061; Additional folding */
                   '\x0070', '\x0061' }),
            new CharMap('\x3373', new char[] { '\x0061',          /* 3373; 0061 0075; Additional folding */
                   '\x0075' }),
            new CharMap('\x3375', new char[] { '\x006F',          /* 3375; 006F 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x3380', new char[] { '\x0070',          /* 3380; 0070 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x3381', new char[] { '\x006E',          /* 3381; 006E 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x3382', new char[] { '\x03BC',          /* 3382; 03BC 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x3383', new char[] { '\x006D',          /* 3383; 006D 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x3384', new char[] { '\x006B',          /* 3384; 006B 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x3385', new char[] { '\x006B',          /* 3385; 006B 0062; Additional folding */
                   '\x0062' }),
            new CharMap('\x3386', new char[] { '\x006D',          /* 3386; 006D 0062; Additional folding */
                   '\x0062' }),
            new CharMap('\x3387', new char[] { '\x0067',          /* 3387; 0067 0062; Additional folding */
                   '\x0062' }),
            new CharMap('\x338A', new char[] { '\x0070',          /* 338A; 0070 0066; Additional folding */
                   '\x0066' }),
            new CharMap('\x338B', new char[] { '\x006E',          /* 338B; 006E 0066; Additional folding */
                   '\x0066' }),
            new CharMap('\x338C', new char[] { '\x03BC',          /* 338C; 03BC 0066; Additional folding */
                   '\x0066' }),
            new CharMap('\x3390', new char[] { '\x0068',          /* 3390; 0068 007A; Additional folding */
                   '\x007A' }),
            new CharMap('\x3391', new char[] { '\x006B',     /* 3391; 006B 0068 007A; Additional folding */
                   '\x0068', '\x007A' }),
            new CharMap('\x3392', new char[] { '\x006D',     /* 3392; 006D 0068 007A; Additional folding */
                   '\x0068', '\x007A' }),
            new CharMap('\x3393', new char[] { '\x0067',     /* 3393; 0067 0068 007A; Additional folding */
                   '\x0068', '\x007A' }),
            new CharMap('\x3394', new char[] { '\x0074',     /* 3394; 0074 0068 007A; Additional folding */
                   '\x0068', '\x007A' }),
            new CharMap('\x33A9', new char[] { '\x0070',          /* 33A9; 0070 0061; Additional folding */
                   '\x0061' }),
            new CharMap('\x33AA', new char[] { '\x006B',     /* 33AA; 006B 0070 0061; Additional folding */
                   '\x0070', '\x0061' }),
            new CharMap('\x33AB', new char[] { '\x006D',     /* 33AB; 006D 0070 0061; Additional folding */
                   '\x0070', '\x0061' }),
            new CharMap('\x33AC', new char[] { '\x0067',     /* 33AC; 0067 0070 0061; Additional folding */
                   '\x0070', '\x0061' }),
            new CharMap('\x33B4', new char[] { '\x0070',          /* 33B4; 0070 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33B5', new char[] { '\x006E',          /* 33B5; 006E 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33B6', new char[] { '\x03BC',          /* 33B6; 03BC 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33B7', new char[] { '\x006D',          /* 33B7; 006D 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33B8', new char[] { '\x006B',          /* 33B8; 006B 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33B9', new char[] { '\x006D',          /* 33B9; 006D 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33BA', new char[] { '\x0070',          /* 33BA; 0070 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33BB', new char[] { '\x006E',          /* 33BB; 006E 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33BC', new char[] { '\x03BC',          /* 33BC; 03BC 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33BD', new char[] { '\x006D',          /* 33BD; 006D 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33BE', new char[] { '\x006B',          /* 33BE; 006B 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33BF', new char[] { '\x006D',          /* 33BF; 006D 0077; Additional folding */
                   '\x0077' }),
            new CharMap('\x33C0', new char[] { '\x006B',          /* 33C0; 006B 03C9; Additional folding */
                   '\x03C9' }),
            new CharMap('\x33C1', new char[] { '\x006D',          /* 33C1; 006D 03C9; Additional folding */
                   '\x03C9' }),
            new CharMap('\x33C3', new char[] { '\x0062',          /* 33C3; 0062 0071; Additional folding */
                   '\x0071' }),
            new CharMap('\x33C6', new char[] { '\x0063', /* 33C6; 0063 2215 006B 0067; Additional folding */
                   '\x2215', '\x006B', '\x0067' }),
            new CharMap('\x33C7', new char[] { '\x0063',     /* 33C7; 0063 006F 002E; Additional folding */
                   '\x006F', '\x002E' }),
            new CharMap('\x33C8', new char[] { '\x0064',          /* 33C8; 0064 0062; Additional folding */
                   '\x0062' }),
            new CharMap('\x33C9', new char[] { '\x0067',          /* 33C9; 0067 0079; Additional folding */
                   '\x0079' }),
            new CharMap('\x33CB', new char[] { '\x0068',          /* 33CB; 0068 0070; Additional folding */
                   '\x0070' }),
            new CharMap('\x33CD', new char[] { '\x006B',          /* 33CD; 006B 006B; Additional folding */
                   '\x006B' }),
            new CharMap('\x33CE', new char[] { '\x006B',          /* 33CE; 006B 006D; Additional folding */
                   '\x006D' }),
            new CharMap('\x33D7', new char[] { '\x0070',          /* 33D7; 0070 0068; Additional folding */
                   '\x0068' }),
            new CharMap('\x33D9', new char[] { '\x0070',     /* 33D9; 0070 0070 006D; Additional folding */
                   '\x0070', '\x006D' }),
            new CharMap('\x33DA', new char[] { '\x0070',          /* 33DA; 0070 0072; Additional folding */
                   '\x0072' }),
            new CharMap('\x33DC', new char[] { '\x0073',          /* 33DC; 0073 0076; Additional folding */
                   '\x0076' }),
            new CharMap('\x33DD', new char[] { '\x0077',          /* 33DD; 0077 0062; Additional folding */
                   '\x0062' }),
            new CharMap('\xFB00', new char[] { '\x0066',                    /* FB00; 0066 0066; Case map */
                   '\x0066' }),
            new CharMap('\xFB01', new char[] { '\x0066',                    /* FB01; 0066 0069; Case map */
                   '\x0069' }),
            new CharMap('\xFB02', new char[] { '\x0066',                    /* FB02; 0066 006C; Case map */
                   '\x006C' }),
            new CharMap('\xFB03', new char[] { '\x0066',               /* FB03; 0066 0066 0069; Case map */
                   '\x0066', '\x0069' }),
            new CharMap('\xFB04', new char[] { '\x0066',               /* FB04; 0066 0066 006C; Case map */
                   '\x0066', '\x006C' }),
            new CharMap('\xFB05', new char[] { '\x0073',                    /* FB05; 0073 0074; Case map */
                   '\x0074' }),
            new CharMap('\xFB06', new char[] { '\x0073',                    /* FB06; 0073 0074; Case map */
                   '\x0074' }),
            new CharMap('\xFB13', new char[] { '\x0574',                    /* FB13; 0574 0576; Case map */
                   '\x0576' }),
            new CharMap('\xFB14', new char[] { '\x0574',                    /* FB14; 0574 0565; Case map */
                   '\x0565' }),
            new CharMap('\xFB15', new char[] { '\x0574',                    /* FB15; 0574 056B; Case map */
                   '\x056B' }),
            new CharMap('\xFB16', new char[] { '\x057E',                    /* FB16; 057E 0576; Case map */
                   '\x0576' }),
            new CharMap('\xFB17', new char[] { '\x0574',                    /* FB17; 0574 056D; Case map */
                   '\x056D' }),
            new CharMap('\xFF21', new char[] { '\xFF41' }),                      /* FF21; FF41; Case map */
            new CharMap('\xFF22', new char[] { '\xFF42' }),                      /* FF22; FF42; Case map */
            new CharMap('\xFF23', new char[] { '\xFF43' }),                      /* FF23; FF43; Case map */
            new CharMap('\xFF24', new char[] { '\xFF44' }),                      /* FF24; FF44; Case map */
            new CharMap('\xFF25', new char[] { '\xFF45' }),                      /* FF25; FF45; Case map */
            new CharMap('\xFF26', new char[] { '\xFF46' }),                      /* FF26; FF46; Case map */
            new CharMap('\xFF27', new char[] { '\xFF47' }),                      /* FF27; FF47; Case map */
            new CharMap('\xFF28', new char[] { '\xFF48' }),                      /* FF28; FF48; Case map */
            new CharMap('\xFF29', new char[] { '\xFF49' }),                      /* FF29; FF49; Case map */
            new CharMap('\xFF2A', new char[] { '\xFF4A' }),                      /* FF2A; FF4A; Case map */
            new CharMap('\xFF2B', new char[] { '\xFF4B' }),                      /* FF2B; FF4B; Case map */
            new CharMap('\xFF2C', new char[] { '\xFF4C' }),                      /* FF2C; FF4C; Case map */
            new CharMap('\xFF2D', new char[] { '\xFF4D' }),                      /* FF2D; FF4D; Case map */
            new CharMap('\xFF2E', new char[] { '\xFF4E' }),                      /* FF2E; FF4E; Case map */
            new CharMap('\xFF2F', new char[] { '\xFF4F' }),                      /* FF2F; FF4F; Case map */
            new CharMap('\xFF30', new char[] { '\xFF50' }),                      /* FF30; FF50; Case map */
            new CharMap('\xFF31', new char[] { '\xFF51' }),                      /* FF31; FF51; Case map */
            new CharMap('\xFF32', new char[] { '\xFF52' }),                      /* FF32; FF52; Case map */
            new CharMap('\xFF33', new char[] { '\xFF53' }),                      /* FF33; FF53; Case map */
            new CharMap('\xFF34', new char[] { '\xFF54' }),                      /* FF34; FF54; Case map */
            new CharMap('\xFF35', new char[] { '\xFF55' }),                      /* FF35; FF55; Case map */
            new CharMap('\xFF36', new char[] { '\xFF56' }),                      /* FF36; FF56; Case map */
            new CharMap('\xFF37', new char[] { '\xFF57' }),                      /* FF37; FF57; Case map */
            new CharMap('\xFF38', new char[] { '\xFF58' }),                      /* FF38; FF58; Case map */
            new CharMap('\xFF39', new char[] { '\xFF59' }),                      /* FF39; FF59; Case map */
            new CharMap('\xFF3A', new char[] { '\xFF5A' }),                      /* FF3A; FF5A; Case map */
        };


        /*
         * B.3 Mapping for case-folding used with no normalization
         * 
         */
        public static readonly CharMap[] B_3 = new CharMap[]
        {
            new CharMap('\x0041', new char[] { '\x0061' }),                      /* 0041; 0061; Case map */
            new CharMap('\x0042', new char[] { '\x0062' }),                      /* 0042; 0062; Case map */
            new CharMap('\x0043', new char[] { '\x0063' }),                      /* 0043; 0063; Case map */
            new CharMap('\x0044', new char[] { '\x0064' }),                      /* 0044; 0064; Case map */
            new CharMap('\x0045', new char[] { '\x0065' }),                      /* 0045; 0065; Case map */
            new CharMap('\x0046', new char[] { '\x0066' }),                      /* 0046; 0066; Case map */
            new CharMap('\x0047', new char[] { '\x0067' }),                      /* 0047; 0067; Case map */
            new CharMap('\x0048', new char[] { '\x0068' }),                      /* 0048; 0068; Case map */
            new CharMap('\x0049', new char[] { '\x0069' }),                      /* 0049; 0069; Case map */
            new CharMap('\x004A', new char[] { '\x006A' }),                      /* 004A; 006A; Case map */
            new CharMap('\x004B', new char[] { '\x006B' }),                      /* 004B; 006B; Case map */
            new CharMap('\x004C', new char[] { '\x006C' }),                      /* 004C; 006C; Case map */
            new CharMap('\x004D', new char[] { '\x006D' }),                      /* 004D; 006D; Case map */
            new CharMap('\x004E', new char[] { '\x006E' }),                      /* 004E; 006E; Case map */
            new CharMap('\x004F', new char[] { '\x006F' }),                      /* 004F; 006F; Case map */
            new CharMap('\x0050', new char[] { '\x0070' }),                      /* 0050; 0070; Case map */
            new CharMap('\x0051', new char[] { '\x0071' }),                      /* 0051; 0071; Case map */
            new CharMap('\x0052', new char[] { '\x0072' }),                      /* 0052; 0072; Case map */
            new CharMap('\x0053', new char[] { '\x0073' }),                      /* 0053; 0073; Case map */
            new CharMap('\x0054', new char[] { '\x0074' }),                      /* 0054; 0074; Case map */
            new CharMap('\x0055', new char[] { '\x0075' }),                      /* 0055; 0075; Case map */
            new CharMap('\x0056', new char[] { '\x0076' }),                      /* 0056; 0076; Case map */
            new CharMap('\x0057', new char[] { '\x0077' }),                      /* 0057; 0077; Case map */
            new CharMap('\x0058', new char[] { '\x0078' }),                      /* 0058; 0078; Case map */
            new CharMap('\x0059', new char[] { '\x0079' }),                      /* 0059; 0079; Case map */
            new CharMap('\x005A', new char[] { '\x007A' }),                      /* 005A; 007A; Case map */
            new CharMap('\x00B5', new char[] { '\x03BC' }),                      /* 00B5; 03BC; Case map */
            new CharMap('\x00C0', new char[] { '\x00E0' }),                      /* 00C0; 00E0; Case map */
            new CharMap('\x00C1', new char[] { '\x00E1' }),                      /* 00C1; 00E1; Case map */
            new CharMap('\x00C2', new char[] { '\x00E2' }),                      /* 00C2; 00E2; Case map */
            new CharMap('\x00C3', new char[] { '\x00E3' }),                      /* 00C3; 00E3; Case map */
            new CharMap('\x00C4', new char[] { '\x00E4' }),                      /* 00C4; 00E4; Case map */
            new CharMap('\x00C5', new char[] { '\x00E5' }),                      /* 00C5; 00E5; Case map */
            new CharMap('\x00C6', new char[] { '\x00E6' }),                      /* 00C6; 00E6; Case map */
            new CharMap('\x00C7', new char[] { '\x00E7' }),                      /* 00C7; 00E7; Case map */
            new CharMap('\x00C8', new char[] { '\x00E8' }),                      /* 00C8; 00E8; Case map */
            new CharMap('\x00C9', new char[] { '\x00E9' }),                      /* 00C9; 00E9; Case map */
            new CharMap('\x00CA', new char[] { '\x00EA' }),                      /* 00CA; 00EA; Case map */
            new CharMap('\x00CB', new char[] { '\x00EB' }),                      /* 00CB; 00EB; Case map */
            new CharMap('\x00CC', new char[] { '\x00EC' }),                      /* 00CC; 00EC; Case map */
            new CharMap('\x00CD', new char[] { '\x00ED' }),                      /* 00CD; 00ED; Case map */
            new CharMap('\x00CE', new char[] { '\x00EE' }),                      /* 00CE; 00EE; Case map */
            new CharMap('\x00CF', new char[] { '\x00EF' }),                      /* 00CF; 00EF; Case map */
            new CharMap('\x00D0', new char[] { '\x00F0' }),                      /* 00D0; 00F0; Case map */
            new CharMap('\x00D1', new char[] { '\x00F1' }),                      /* 00D1; 00F1; Case map */
            new CharMap('\x00D2', new char[] { '\x00F2' }),                      /* 00D2; 00F2; Case map */
            new CharMap('\x00D3', new char[] { '\x00F3' }),                      /* 00D3; 00F3; Case map */
            new CharMap('\x00D4', new char[] { '\x00F4' }),                      /* 00D4; 00F4; Case map */
            new CharMap('\x00D5', new char[] { '\x00F5' }),                      /* 00D5; 00F5; Case map */
            new CharMap('\x00D6', new char[] { '\x00F6' }),                      /* 00D6; 00F6; Case map */
            new CharMap('\x00D8', new char[] { '\x00F8' }),                      /* 00D8; 00F8; Case map */
            new CharMap('\x00D9', new char[] { '\x00F9' }),                      /* 00D9; 00F9; Case map */
            new CharMap('\x00DA', new char[] { '\x00FA' }),                      /* 00DA; 00FA; Case map */
            new CharMap('\x00DB', new char[] { '\x00FB' }),                      /* 00DB; 00FB; Case map */
            new CharMap('\x00DC', new char[] { '\x00FC' }),                      /* 00DC; 00FC; Case map */
            new CharMap('\x00DD', new char[] { '\x00FD' }),                      /* 00DD; 00FD; Case map */
            new CharMap('\x00DE', new char[] { '\x00FE' }),                      /* 00DE; 00FE; Case map */
            new CharMap('\x00DF', new char[] { '\x0073',                    /* 00DF; 0073 0073; Case map */
                   '\x0073' }),
            new CharMap('\x0100', new char[] { '\x0101' }),                      /* 0100; 0101; Case map */
            new CharMap('\x0102', new char[] { '\x0103' }),                      /* 0102; 0103; Case map */
            new CharMap('\x0104', new char[] { '\x0105' }),                      /* 0104; 0105; Case map */
            new CharMap('\x0106', new char[] { '\x0107' }),                      /* 0106; 0107; Case map */
            new CharMap('\x0108', new char[] { '\x0109' }),                      /* 0108; 0109; Case map */
            new CharMap('\x010A', new char[] { '\x010B' }),                      /* 010A; 010B; Case map */
            new CharMap('\x010C', new char[] { '\x010D' }),                      /* 010C; 010D; Case map */
            new CharMap('\x010E', new char[] { '\x010F' }),                      /* 010E; 010F; Case map */
            new CharMap('\x0110', new char[] { '\x0111' }),                      /* 0110; 0111; Case map */
            new CharMap('\x0112', new char[] { '\x0113' }),                      /* 0112; 0113; Case map */
            new CharMap('\x0114', new char[] { '\x0115' }),                      /* 0114; 0115; Case map */
            new CharMap('\x0116', new char[] { '\x0117' }),                      /* 0116; 0117; Case map */
            new CharMap('\x0118', new char[] { '\x0119' }),                      /* 0118; 0119; Case map */
            new CharMap('\x011A', new char[] { '\x011B' }),                      /* 011A; 011B; Case map */
            new CharMap('\x011C', new char[] { '\x011D' }),                      /* 011C; 011D; Case map */
            new CharMap('\x011E', new char[] { '\x011F' }),                      /* 011E; 011F; Case map */
            new CharMap('\x0120', new char[] { '\x0121' }),                      /* 0120; 0121; Case map */
            new CharMap('\x0122', new char[] { '\x0123' }),                      /* 0122; 0123; Case map */
            new CharMap('\x0124', new char[] { '\x0125' }),                      /* 0124; 0125; Case map */
            new CharMap('\x0126', new char[] { '\x0127' }),                      /* 0126; 0127; Case map */
            new CharMap('\x0128', new char[] { '\x0129' }),                      /* 0128; 0129; Case map */
            new CharMap('\x012A', new char[] { '\x012B' }),                      /* 012A; 012B; Case map */
            new CharMap('\x012C', new char[] { '\x012D' }),                      /* 012C; 012D; Case map */
            new CharMap('\x012E', new char[] { '\x012F' }),                      /* 012E; 012F; Case map */
            new CharMap('\x0130', new char[] { '\x0069',                    /* 0130; 0069 0307; Case map */
                   '\x0307' }),
            new CharMap('\x0132', new char[] { '\x0133' }),                      /* 0132; 0133; Case map */
            new CharMap('\x0134', new char[] { '\x0135' }),                      /* 0134; 0135; Case map */
            new CharMap('\x0136', new char[] { '\x0137' }),                      /* 0136; 0137; Case map */
            new CharMap('\x0139', new char[] { '\x013A' }),                      /* 0139; 013A; Case map */
            new CharMap('\x013B', new char[] { '\x013C' }),                      /* 013B; 013C; Case map */
            new CharMap('\x013D', new char[] { '\x013E' }),                      /* 013D; 013E; Case map */
            new CharMap('\x013F', new char[] { '\x0140' }),                      /* 013F; 0140; Case map */
            new CharMap('\x0141', new char[] { '\x0142' }),                      /* 0141; 0142; Case map */
            new CharMap('\x0143', new char[] { '\x0144' }),                      /* 0143; 0144; Case map */
            new CharMap('\x0145', new char[] { '\x0146' }),                      /* 0145; 0146; Case map */
            new CharMap('\x0147', new char[] { '\x0148' }),                      /* 0147; 0148; Case map */
            new CharMap('\x0149', new char[] { '\x02BC',                    /* 0149; 02BC 006E; Case map */
                   '\x006E' }),
            new CharMap('\x014A', new char[] { '\x014B' }),                      /* 014A; 014B; Case map */
            new CharMap('\x014C', new char[] { '\x014D' }),                      /* 014C; 014D; Case map */
            new CharMap('\x014E', new char[] { '\x014F' }),                      /* 014E; 014F; Case map */
            new CharMap('\x0150', new char[] { '\x0151' }),                      /* 0150; 0151; Case map */
            new CharMap('\x0152', new char[] { '\x0153' }),                      /* 0152; 0153; Case map */
            new CharMap('\x0154', new char[] { '\x0155' }),                      /* 0154; 0155; Case map */
            new CharMap('\x0156', new char[] { '\x0157' }),                      /* 0156; 0157; Case map */
            new CharMap('\x0158', new char[] { '\x0159' }),                      /* 0158; 0159; Case map */
            new CharMap('\x015A', new char[] { '\x015B' }),                      /* 015A; 015B; Case map */
            new CharMap('\x015C', new char[] { '\x015D' }),                      /* 015C; 015D; Case map */
            new CharMap('\x015E', new char[] { '\x015F' }),                      /* 015E; 015F; Case map */
            new CharMap('\x0160', new char[] { '\x0161' }),                      /* 0160; 0161; Case map */
            new CharMap('\x0162', new char[] { '\x0163' }),                      /* 0162; 0163; Case map */
            new CharMap('\x0164', new char[] { '\x0165' }),                      /* 0164; 0165; Case map */
            new CharMap('\x0166', new char[] { '\x0167' }),                      /* 0166; 0167; Case map */
            new CharMap('\x0168', new char[] { '\x0169' }),                      /* 0168; 0169; Case map */
            new CharMap('\x016A', new char[] { '\x016B' }),                      /* 016A; 016B; Case map */
            new CharMap('\x016C', new char[] { '\x016D' }),                      /* 016C; 016D; Case map */
            new CharMap('\x016E', new char[] { '\x016F' }),                      /* 016E; 016F; Case map */
            new CharMap('\x0170', new char[] { '\x0171' }),                      /* 0170; 0171; Case map */
            new CharMap('\x0172', new char[] { '\x0173' }),                      /* 0172; 0173; Case map */
            new CharMap('\x0174', new char[] { '\x0175' }),                      /* 0174; 0175; Case map */
            new CharMap('\x0176', new char[] { '\x0177' }),                      /* 0176; 0177; Case map */
            new CharMap('\x0178', new char[] { '\x00FF' }),                      /* 0178; 00FF; Case map */
            new CharMap('\x0179', new char[] { '\x017A' }),                      /* 0179; 017A; Case map */
            new CharMap('\x017B', new char[] { '\x017C' }),                      /* 017B; 017C; Case map */
            new CharMap('\x017D', new char[] { '\x017E' }),                      /* 017D; 017E; Case map */
            new CharMap('\x017F', new char[] { '\x0073' }),                      /* 017F; 0073; Case map */
            new CharMap('\x0181', new char[] { '\x0253' }),                      /* 0181; 0253; Case map */
            new CharMap('\x0182', new char[] { '\x0183' }),                      /* 0182; 0183; Case map */
            new CharMap('\x0184', new char[] { '\x0185' }),                      /* 0184; 0185; Case map */
            new CharMap('\x0186', new char[] { '\x0254' }),                      /* 0186; 0254; Case map */
            new CharMap('\x0187', new char[] { '\x0188' }),                      /* 0187; 0188; Case map */
            new CharMap('\x0189', new char[] { '\x0256' }),                      /* 0189; 0256; Case map */
            new CharMap('\x018A', new char[] { '\x0257' }),                      /* 018A; 0257; Case map */
            new CharMap('\x018B', new char[] { '\x018C' }),                      /* 018B; 018C; Case map */
            new CharMap('\x018E', new char[] { '\x01DD' }),                      /* 018E; 01DD; Case map */
            new CharMap('\x018F', new char[] { '\x0259' }),                      /* 018F; 0259; Case map */
            new CharMap('\x0190', new char[] { '\x025B' }),                      /* 0190; 025B; Case map */
            new CharMap('\x0191', new char[] { '\x0192' }),                      /* 0191; 0192; Case map */
            new CharMap('\x0193', new char[] { '\x0260' }),                      /* 0193; 0260; Case map */
            new CharMap('\x0194', new char[] { '\x0263' }),                      /* 0194; 0263; Case map */
            new CharMap('\x0196', new char[] { '\x0269' }),                      /* 0196; 0269; Case map */
            new CharMap('\x0197', new char[] { '\x0268' }),                      /* 0197; 0268; Case map */
            new CharMap('\x0198', new char[] { '\x0199' }),                      /* 0198; 0199; Case map */
            new CharMap('\x019C', new char[] { '\x026F' }),                      /* 019C; 026F; Case map */
            new CharMap('\x019D', new char[] { '\x0272' }),                      /* 019D; 0272; Case map */
            new CharMap('\x019F', new char[] { '\x0275' }),                      /* 019F; 0275; Case map */
            new CharMap('\x01A0', new char[] { '\x01A1' }),                      /* 01A0; 01A1; Case map */
            new CharMap('\x01A2', new char[] { '\x01A3' }),                      /* 01A2; 01A3; Case map */
            new CharMap('\x01A4', new char[] { '\x01A5' }),                      /* 01A4; 01A5; Case map */
            new CharMap('\x01A6', new char[] { '\x0280' }),                      /* 01A6; 0280; Case map */
            new CharMap('\x01A7', new char[] { '\x01A8' }),                      /* 01A7; 01A8; Case map */
            new CharMap('\x01A9', new char[] { '\x0283' }),                      /* 01A9; 0283; Case map */
            new CharMap('\x01AC', new char[] { '\x01AD' }),                      /* 01AC; 01AD; Case map */
            new CharMap('\x01AE', new char[] { '\x0288' }),                      /* 01AE; 0288; Case map */
            new CharMap('\x01AF', new char[] { '\x01B0' }),                      /* 01AF; 01B0; Case map */
            new CharMap('\x01B1', new char[] { '\x028A' }),                      /* 01B1; 028A; Case map */
            new CharMap('\x01B2', new char[] { '\x028B' }),                      /* 01B2; 028B; Case map */
            new CharMap('\x01B3', new char[] { '\x01B4' }),                      /* 01B3; 01B4; Case map */
            new CharMap('\x01B5', new char[] { '\x01B6' }),                      /* 01B5; 01B6; Case map */
            new CharMap('\x01B7', new char[] { '\x0292' }),                      /* 01B7; 0292; Case map */
            new CharMap('\x01B8', new char[] { '\x01B9' }),                      /* 01B8; 01B9; Case map */
            new CharMap('\x01BC', new char[] { '\x01BD' }),                      /* 01BC; 01BD; Case map */
            new CharMap('\x01C4', new char[] { '\x01C6' }),                      /* 01C4; 01C6; Case map */
            new CharMap('\x01C5', new char[] { '\x01C6' }),                      /* 01C5; 01C6; Case map */
            new CharMap('\x01C7', new char[] { '\x01C9' }),                      /* 01C7; 01C9; Case map */
            new CharMap('\x01C8', new char[] { '\x01C9' }),                      /* 01C8; 01C9; Case map */
            new CharMap('\x01CA', new char[] { '\x01CC' }),                      /* 01CA; 01CC; Case map */
            new CharMap('\x01CB', new char[] { '\x01CC' }),                      /* 01CB; 01CC; Case map */
            new CharMap('\x01CD', new char[] { '\x01CE' }),                      /* 01CD; 01CE; Case map */
            new CharMap('\x01CF', new char[] { '\x01D0' }),                      /* 01CF; 01D0; Case map */
            new CharMap('\x01D1', new char[] { '\x01D2' }),                      /* 01D1; 01D2; Case map */
            new CharMap('\x01D3', new char[] { '\x01D4' }),                      /* 01D3; 01D4; Case map */
            new CharMap('\x01D5', new char[] { '\x01D6' }),                      /* 01D5; 01D6; Case map */
            new CharMap('\x01D7', new char[] { '\x01D8' }),                      /* 01D7; 01D8; Case map */
            new CharMap('\x01D9', new char[] { '\x01DA' }),                      /* 01D9; 01DA; Case map */
            new CharMap('\x01DB', new char[] { '\x01DC' }),                      /* 01DB; 01DC; Case map */
            new CharMap('\x01DE', new char[] { '\x01DF' }),                      /* 01DE; 01DF; Case map */
            new CharMap('\x01E0', new char[] { '\x01E1' }),                      /* 01E0; 01E1; Case map */
            new CharMap('\x01E2', new char[] { '\x01E3' }),                      /* 01E2; 01E3; Case map */
            new CharMap('\x01E4', new char[] { '\x01E5' }),                      /* 01E4; 01E5; Case map */
            new CharMap('\x01E6', new char[] { '\x01E7' }),                      /* 01E6; 01E7; Case map */
            new CharMap('\x01E8', new char[] { '\x01E9' }),                      /* 01E8; 01E9; Case map */
            new CharMap('\x01EA', new char[] { '\x01EB' }),                      /* 01EA; 01EB; Case map */
            new CharMap('\x01EC', new char[] { '\x01ED' }),                      /* 01EC; 01ED; Case map */
            new CharMap('\x01EE', new char[] { '\x01EF' }),                      /* 01EE; 01EF; Case map */
            new CharMap('\x01F0', new char[] { '\x006A',                    /* 01F0; 006A 030C; Case map */
                   '\x030C' }),
            new CharMap('\x01F1', new char[] { '\x01F3' }),                      /* 01F1; 01F3; Case map */
            new CharMap('\x01F2', new char[] { '\x01F3' }),                      /* 01F2; 01F3; Case map */
            new CharMap('\x01F4', new char[] { '\x01F5' }),                      /* 01F4; 01F5; Case map */
            new CharMap('\x01F6', new char[] { '\x0195' }),                      /* 01F6; 0195; Case map */
            new CharMap('\x01F7', new char[] { '\x01BF' }),                      /* 01F7; 01BF; Case map */
            new CharMap('\x01F8', new char[] { '\x01F9' }),                      /* 01F8; 01F9; Case map */
            new CharMap('\x01FA', new char[] { '\x01FB' }),                      /* 01FA; 01FB; Case map */
            new CharMap('\x01FC', new char[] { '\x01FD' }),                      /* 01FC; 01FD; Case map */
            new CharMap('\x01FE', new char[] { '\x01FF' }),                      /* 01FE; 01FF; Case map */
            new CharMap('\x0200', new char[] { '\x0201' }),                      /* 0200; 0201; Case map */
            new CharMap('\x0202', new char[] { '\x0203' }),                      /* 0202; 0203; Case map */
            new CharMap('\x0204', new char[] { '\x0205' }),                      /* 0204; 0205; Case map */
            new CharMap('\x0206', new char[] { '\x0207' }),                      /* 0206; 0207; Case map */
            new CharMap('\x0208', new char[] { '\x0209' }),                      /* 0208; 0209; Case map */
            new CharMap('\x020A', new char[] { '\x020B' }),                      /* 020A; 020B; Case map */
            new CharMap('\x020C', new char[] { '\x020D' }),                      /* 020C; 020D; Case map */
            new CharMap('\x020E', new char[] { '\x020F' }),                      /* 020E; 020F; Case map */
            new CharMap('\x0210', new char[] { '\x0211' }),                      /* 0210; 0211; Case map */
            new CharMap('\x0212', new char[] { '\x0213' }),                      /* 0212; 0213; Case map */
            new CharMap('\x0214', new char[] { '\x0215' }),                      /* 0214; 0215; Case map */
            new CharMap('\x0216', new char[] { '\x0217' }),                      /* 0216; 0217; Case map */
            new CharMap('\x0218', new char[] { '\x0219' }),                      /* 0218; 0219; Case map */
            new CharMap('\x021A', new char[] { '\x021B' }),                      /* 021A; 021B; Case map */
            new CharMap('\x021C', new char[] { '\x021D' }),                      /* 021C; 021D; Case map */
            new CharMap('\x021E', new char[] { '\x021F' }),                      /* 021E; 021F; Case map */
            new CharMap('\x0220', new char[] { '\x019E' }),                      /* 0220; 019E; Case map */
            new CharMap('\x0222', new char[] { '\x0223' }),                      /* 0222; 0223; Case map */
            new CharMap('\x0224', new char[] { '\x0225' }),                      /* 0224; 0225; Case map */
            new CharMap('\x0226', new char[] { '\x0227' }),                      /* 0226; 0227; Case map */
            new CharMap('\x0228', new char[] { '\x0229' }),                      /* 0228; 0229; Case map */
            new CharMap('\x022A', new char[] { '\x022B' }),                      /* 022A; 022B; Case map */
            new CharMap('\x022C', new char[] { '\x022D' }),                      /* 022C; 022D; Case map */
            new CharMap('\x022E', new char[] { '\x022F' }),                      /* 022E; 022F; Case map */
            new CharMap('\x0230', new char[] { '\x0231' }),                      /* 0230; 0231; Case map */
            new CharMap('\x0232', new char[] { '\x0233' }),                      /* 0232; 0233; Case map */
            new CharMap('\x0345', new char[] { '\x03B9' }),                      /* 0345; 03B9; Case map */
            new CharMap('\x0386', new char[] { '\x03AC' }),                      /* 0386; 03AC; Case map */
            new CharMap('\x0388', new char[] { '\x03AD' }),                      /* 0388; 03AD; Case map */
            new CharMap('\x0389', new char[] { '\x03AE' }),                      /* 0389; 03AE; Case map */
            new CharMap('\x038A', new char[] { '\x03AF' }),                      /* 038A; 03AF; Case map */
            new CharMap('\x038C', new char[] { '\x03CC' }),                      /* 038C; 03CC; Case map */
            new CharMap('\x038E', new char[] { '\x03CD' }),                      /* 038E; 03CD; Case map */
            new CharMap('\x038F', new char[] { '\x03CE' }),                      /* 038F; 03CE; Case map */
            new CharMap('\x0390', new char[] { '\x03B9',               /* 0390; 03B9 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x0391', new char[] { '\x03B1' }),                      /* 0391; 03B1; Case map */
            new CharMap('\x0392', new char[] { '\x03B2' }),                      /* 0392; 03B2; Case map */
            new CharMap('\x0393', new char[] { '\x03B3' }),                      /* 0393; 03B3; Case map */
            new CharMap('\x0394', new char[] { '\x03B4' }),                      /* 0394; 03B4; Case map */
            new CharMap('\x0395', new char[] { '\x03B5' }),                      /* 0395; 03B5; Case map */
            new CharMap('\x0396', new char[] { '\x03B6' }),                      /* 0396; 03B6; Case map */
            new CharMap('\x0397', new char[] { '\x03B7' }),                      /* 0397; 03B7; Case map */
            new CharMap('\x0398', new char[] { '\x03B8' }),                      /* 0398; 03B8; Case map */
            new CharMap('\x0399', new char[] { '\x03B9' }),                      /* 0399; 03B9; Case map */
            new CharMap('\x039A', new char[] { '\x03BA' }),                      /* 039A; 03BA; Case map */
            new CharMap('\x039B', new char[] { '\x03BB' }),                      /* 039B; 03BB; Case map */
            new CharMap('\x039C', new char[] { '\x03BC' }),                      /* 039C; 03BC; Case map */
            new CharMap('\x039D', new char[] { '\x03BD' }),                      /* 039D; 03BD; Case map */
            new CharMap('\x039E', new char[] { '\x03BE' }),                      /* 039E; 03BE; Case map */
            new CharMap('\x039F', new char[] { '\x03BF' }),                      /* 039F; 03BF; Case map */
            new CharMap('\x03A0', new char[] { '\x03C0' }),                      /* 03A0; 03C0; Case map */
            new CharMap('\x03A1', new char[] { '\x03C1' }),                      /* 03A1; 03C1; Case map */
            new CharMap('\x03A3', new char[] { '\x03C3' }),                      /* 03A3; 03C3; Case map */
            new CharMap('\x03A4', new char[] { '\x03C4' }),                      /* 03A4; 03C4; Case map */
            new CharMap('\x03A5', new char[] { '\x03C5' }),                      /* 03A5; 03C5; Case map */
            new CharMap('\x03A6', new char[] { '\x03C6' }),                      /* 03A6; 03C6; Case map */
            new CharMap('\x03A7', new char[] { '\x03C7' }),                      /* 03A7; 03C7; Case map */
            new CharMap('\x03A8', new char[] { '\x03C8' }),                      /* 03A8; 03C8; Case map */
            new CharMap('\x03A9', new char[] { '\x03C9' }),                      /* 03A9; 03C9; Case map */
            new CharMap('\x03AA', new char[] { '\x03CA' }),                      /* 03AA; 03CA; Case map */
            new CharMap('\x03AB', new char[] { '\x03CB' }),                      /* 03AB; 03CB; Case map */
            new CharMap('\x03B0', new char[] { '\x03C5',               /* 03B0; 03C5 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x03C2', new char[] { '\x03C3' }),                      /* 03C2; 03C3; Case map */
            new CharMap('\x03D0', new char[] { '\x03B2' }),                      /* 03D0; 03B2; Case map */
            new CharMap('\x03D1', new char[] { '\x03B8' }),                      /* 03D1; 03B8; Case map */
            new CharMap('\x03D5', new char[] { '\x03C6' }),                      /* 03D5; 03C6; Case map */
            new CharMap('\x03D6', new char[] { '\x03C0' }),                      /* 03D6; 03C0; Case map */
            new CharMap('\x03D8', new char[] { '\x03D9' }),                      /* 03D8; 03D9; Case map */
            new CharMap('\x03DA', new char[] { '\x03DB' }),                      /* 03DA; 03DB; Case map */
            new CharMap('\x03DC', new char[] { '\x03DD' }),                      /* 03DC; 03DD; Case map */
            new CharMap('\x03DE', new char[] { '\x03DF' }),                      /* 03DE; 03DF; Case map */
            new CharMap('\x03E0', new char[] { '\x03E1' }),                      /* 03E0; 03E1; Case map */
            new CharMap('\x03E2', new char[] { '\x03E3' }),                      /* 03E2; 03E3; Case map */
            new CharMap('\x03E4', new char[] { '\x03E5' }),                      /* 03E4; 03E5; Case map */
            new CharMap('\x03E6', new char[] { '\x03E7' }),                      /* 03E6; 03E7; Case map */
            new CharMap('\x03E8', new char[] { '\x03E9' }),                      /* 03E8; 03E9; Case map */
            new CharMap('\x03EA', new char[] { '\x03EB' }),                      /* 03EA; 03EB; Case map */
            new CharMap('\x03EC', new char[] { '\x03ED' }),                      /* 03EC; 03ED; Case map */
            new CharMap('\x03EE', new char[] { '\x03EF' }),                      /* 03EE; 03EF; Case map */
            new CharMap('\x03F0', new char[] { '\x03BA' }),                      /* 03F0; 03BA; Case map */
            new CharMap('\x03F1', new char[] { '\x03C1' }),                      /* 03F1; 03C1; Case map */
            new CharMap('\x03F2', new char[] { '\x03C3' }),                      /* 03F2; 03C3; Case map */
            new CharMap('\x03F4', new char[] { '\x03B8' }),                      /* 03F4; 03B8; Case map */
            new CharMap('\x03F5', new char[] { '\x03B5' }),                      /* 03F5; 03B5; Case map */
            new CharMap('\x0400', new char[] { '\x0450' }),                      /* 0400; 0450; Case map */
            new CharMap('\x0401', new char[] { '\x0451' }),                      /* 0401; 0451; Case map */
            new CharMap('\x0402', new char[] { '\x0452' }),                      /* 0402; 0452; Case map */
            new CharMap('\x0403', new char[] { '\x0453' }),                      /* 0403; 0453; Case map */
            new CharMap('\x0404', new char[] { '\x0454' }),                      /* 0404; 0454; Case map */
            new CharMap('\x0405', new char[] { '\x0455' }),                      /* 0405; 0455; Case map */
            new CharMap('\x0406', new char[] { '\x0456' }),                      /* 0406; 0456; Case map */
            new CharMap('\x0407', new char[] { '\x0457' }),                      /* 0407; 0457; Case map */
            new CharMap('\x0408', new char[] { '\x0458' }),                      /* 0408; 0458; Case map */
            new CharMap('\x0409', new char[] { '\x0459' }),                      /* 0409; 0459; Case map */
            new CharMap('\x040A', new char[] { '\x045A' }),                      /* 040A; 045A; Case map */
            new CharMap('\x040B', new char[] { '\x045B' }),                      /* 040B; 045B; Case map */
            new CharMap('\x040C', new char[] { '\x045C' }),                      /* 040C; 045C; Case map */
            new CharMap('\x040D', new char[] { '\x045D' }),                      /* 040D; 045D; Case map */
            new CharMap('\x040E', new char[] { '\x045E' }),                      /* 040E; 045E; Case map */
            new CharMap('\x040F', new char[] { '\x045F' }),                      /* 040F; 045F; Case map */
            new CharMap('\x0410', new char[] { '\x0430' }),                      /* 0410; 0430; Case map */
            new CharMap('\x0411', new char[] { '\x0431' }),                      /* 0411; 0431; Case map */
            new CharMap('\x0412', new char[] { '\x0432' }),                      /* 0412; 0432; Case map */
            new CharMap('\x0413', new char[] { '\x0433' }),                      /* 0413; 0433; Case map */
            new CharMap('\x0414', new char[] { '\x0434' }),                      /* 0414; 0434; Case map */
            new CharMap('\x0415', new char[] { '\x0435' }),                      /* 0415; 0435; Case map */
            new CharMap('\x0416', new char[] { '\x0436' }),                      /* 0416; 0436; Case map */
            new CharMap('\x0417', new char[] { '\x0437' }),                      /* 0417; 0437; Case map */
            new CharMap('\x0418', new char[] { '\x0438' }),                      /* 0418; 0438; Case map */
            new CharMap('\x0419', new char[] { '\x0439' }),                      /* 0419; 0439; Case map */
            new CharMap('\x041A', new char[] { '\x043A' }),                      /* 041A; 043A; Case map */
            new CharMap('\x041B', new char[] { '\x043B' }),                      /* 041B; 043B; Case map */
            new CharMap('\x041C', new char[] { '\x043C' }),                      /* 041C; 043C; Case map */
            new CharMap('\x041D', new char[] { '\x043D' }),                      /* 041D; 043D; Case map */
            new CharMap('\x041E', new char[] { '\x043E' }),                      /* 041E; 043E; Case map */
            new CharMap('\x041F', new char[] { '\x043F' }),                      /* 041F; 043F; Case map */
            new CharMap('\x0420', new char[] { '\x0440' }),                      /* 0420; 0440; Case map */
            new CharMap('\x0421', new char[] { '\x0441' }),                      /* 0421; 0441; Case map */
            new CharMap('\x0422', new char[] { '\x0442' }),                      /* 0422; 0442; Case map */
            new CharMap('\x0423', new char[] { '\x0443' }),                      /* 0423; 0443; Case map */
            new CharMap('\x0424', new char[] { '\x0444' }),                      /* 0424; 0444; Case map */
            new CharMap('\x0425', new char[] { '\x0445' }),                      /* 0425; 0445; Case map */
            new CharMap('\x0426', new char[] { '\x0446' }),                      /* 0426; 0446; Case map */
            new CharMap('\x0427', new char[] { '\x0447' }),                      /* 0427; 0447; Case map */
            new CharMap('\x0428', new char[] { '\x0448' }),                      /* 0428; 0448; Case map */
            new CharMap('\x0429', new char[] { '\x0449' }),                      /* 0429; 0449; Case map */
            new CharMap('\x042A', new char[] { '\x044A' }),                      /* 042A; 044A; Case map */
            new CharMap('\x042B', new char[] { '\x044B' }),                      /* 042B; 044B; Case map */
            new CharMap('\x042C', new char[] { '\x044C' }),                      /* 042C; 044C; Case map */
            new CharMap('\x042D', new char[] { '\x044D' }),                      /* 042D; 044D; Case map */
            new CharMap('\x042E', new char[] { '\x044E' }),                      /* 042E; 044E; Case map */
            new CharMap('\x042F', new char[] { '\x044F' }),                      /* 042F; 044F; Case map */
            new CharMap('\x0460', new char[] { '\x0461' }),                      /* 0460; 0461; Case map */
            new CharMap('\x0462', new char[] { '\x0463' }),                      /* 0462; 0463; Case map */
            new CharMap('\x0464', new char[] { '\x0465' }),                      /* 0464; 0465; Case map */
            new CharMap('\x0466', new char[] { '\x0467' }),                      /* 0466; 0467; Case map */
            new CharMap('\x0468', new char[] { '\x0469' }),                      /* 0468; 0469; Case map */
            new CharMap('\x046A', new char[] { '\x046B' }),                      /* 046A; 046B; Case map */
            new CharMap('\x046C', new char[] { '\x046D' }),                      /* 046C; 046D; Case map */
            new CharMap('\x046E', new char[] { '\x046F' }),                      /* 046E; 046F; Case map */
            new CharMap('\x0470', new char[] { '\x0471' }),                      /* 0470; 0471; Case map */
            new CharMap('\x0472', new char[] { '\x0473' }),                      /* 0472; 0473; Case map */
            new CharMap('\x0474', new char[] { '\x0475' }),                      /* 0474; 0475; Case map */
            new CharMap('\x0476', new char[] { '\x0477' }),                      /* 0476; 0477; Case map */
            new CharMap('\x0478', new char[] { '\x0479' }),                      /* 0478; 0479; Case map */
            new CharMap('\x047A', new char[] { '\x047B' }),                      /* 047A; 047B; Case map */
            new CharMap('\x047C', new char[] { '\x047D' }),                      /* 047C; 047D; Case map */
            new CharMap('\x047E', new char[] { '\x047F' }),                      /* 047E; 047F; Case map */
            new CharMap('\x0480', new char[] { '\x0481' }),                      /* 0480; 0481; Case map */
            new CharMap('\x048A', new char[] { '\x048B' }),                      /* 048A; 048B; Case map */
            new CharMap('\x048C', new char[] { '\x048D' }),                      /* 048C; 048D; Case map */
            new CharMap('\x048E', new char[] { '\x048F' }),                      /* 048E; 048F; Case map */
            new CharMap('\x0490', new char[] { '\x0491' }),                      /* 0490; 0491; Case map */
            new CharMap('\x0492', new char[] { '\x0493' }),                      /* 0492; 0493; Case map */
            new CharMap('\x0494', new char[] { '\x0495' }),                      /* 0494; 0495; Case map */
            new CharMap('\x0496', new char[] { '\x0497' }),                      /* 0496; 0497; Case map */
            new CharMap('\x0498', new char[] { '\x0499' }),                      /* 0498; 0499; Case map */
            new CharMap('\x049A', new char[] { '\x049B' }),                      /* 049A; 049B; Case map */
            new CharMap('\x049C', new char[] { '\x049D' }),                      /* 049C; 049D; Case map */
            new CharMap('\x049E', new char[] { '\x049F' }),                      /* 049E; 049F; Case map */
            new CharMap('\x04A0', new char[] { '\x04A1' }),                      /* 04A0; 04A1; Case map */
            new CharMap('\x04A2', new char[] { '\x04A3' }),                      /* 04A2; 04A3; Case map */
            new CharMap('\x04A4', new char[] { '\x04A5' }),                      /* 04A4; 04A5; Case map */
            new CharMap('\x04A6', new char[] { '\x04A7' }),                      /* 04A6; 04A7; Case map */
            new CharMap('\x04A8', new char[] { '\x04A9' }),                      /* 04A8; 04A9; Case map */
            new CharMap('\x04AA', new char[] { '\x04AB' }),                      /* 04AA; 04AB; Case map */
            new CharMap('\x04AC', new char[] { '\x04AD' }),                      /* 04AC; 04AD; Case map */
            new CharMap('\x04AE', new char[] { '\x04AF' }),                      /* 04AE; 04AF; Case map */
            new CharMap('\x04B0', new char[] { '\x04B1' }),                      /* 04B0; 04B1; Case map */
            new CharMap('\x04B2', new char[] { '\x04B3' }),                      /* 04B2; 04B3; Case map */
            new CharMap('\x04B4', new char[] { '\x04B5' }),                      /* 04B4; 04B5; Case map */
            new CharMap('\x04B6', new char[] { '\x04B7' }),                      /* 04B6; 04B7; Case map */
            new CharMap('\x04B8', new char[] { '\x04B9' }),                      /* 04B8; 04B9; Case map */
            new CharMap('\x04BA', new char[] { '\x04BB' }),                      /* 04BA; 04BB; Case map */
            new CharMap('\x04BC', new char[] { '\x04BD' }),                      /* 04BC; 04BD; Case map */
            new CharMap('\x04BE', new char[] { '\x04BF' }),                      /* 04BE; 04BF; Case map */
            new CharMap('\x04C1', new char[] { '\x04C2' }),                      /* 04C1; 04C2; Case map */
            new CharMap('\x04C3', new char[] { '\x04C4' }),                      /* 04C3; 04C4; Case map */
            new CharMap('\x04C5', new char[] { '\x04C6' }),                      /* 04C5; 04C6; Case map */
            new CharMap('\x04C7', new char[] { '\x04C8' }),                      /* 04C7; 04C8; Case map */
            new CharMap('\x04C9', new char[] { '\x04CA' }),                      /* 04C9; 04CA; Case map */
            new CharMap('\x04CB', new char[] { '\x04CC' }),                      /* 04CB; 04CC; Case map */
            new CharMap('\x04CD', new char[] { '\x04CE' }),                      /* 04CD; 04CE; Case map */
            new CharMap('\x04D0', new char[] { '\x04D1' }),                      /* 04D0; 04D1; Case map */
            new CharMap('\x04D2', new char[] { '\x04D3' }),                      /* 04D2; 04D3; Case map */
            new CharMap('\x04D4', new char[] { '\x04D5' }),                      /* 04D4; 04D5; Case map */
            new CharMap('\x04D6', new char[] { '\x04D7' }),                      /* 04D6; 04D7; Case map */
            new CharMap('\x04D8', new char[] { '\x04D9' }),                      /* 04D8; 04D9; Case map */
            new CharMap('\x04DA', new char[] { '\x04DB' }),                      /* 04DA; 04DB; Case map */
            new CharMap('\x04DC', new char[] { '\x04DD' }),                      /* 04DC; 04DD; Case map */
            new CharMap('\x04DE', new char[] { '\x04DF' }),                      /* 04DE; 04DF; Case map */
            new CharMap('\x04E0', new char[] { '\x04E1' }),                      /* 04E0; 04E1; Case map */
            new CharMap('\x04E2', new char[] { '\x04E3' }),                      /* 04E2; 04E3; Case map */
            new CharMap('\x04E4', new char[] { '\x04E5' }),                      /* 04E4; 04E5; Case map */
            new CharMap('\x04E6', new char[] { '\x04E7' }),                      /* 04E6; 04E7; Case map */
            new CharMap('\x04E8', new char[] { '\x04E9' }),                      /* 04E8; 04E9; Case map */
            new CharMap('\x04EA', new char[] { '\x04EB' }),                      /* 04EA; 04EB; Case map */
            new CharMap('\x04EC', new char[] { '\x04ED' }),                      /* 04EC; 04ED; Case map */
            new CharMap('\x04EE', new char[] { '\x04EF' }),                      /* 04EE; 04EF; Case map */
            new CharMap('\x04F0', new char[] { '\x04F1' }),                      /* 04F0; 04F1; Case map */
            new CharMap('\x04F2', new char[] { '\x04F3' }),                      /* 04F2; 04F3; Case map */
            new CharMap('\x04F4', new char[] { '\x04F5' }),                      /* 04F4; 04F5; Case map */
            new CharMap('\x04F8', new char[] { '\x04F9' }),                      /* 04F8; 04F9; Case map */
            new CharMap('\x0500', new char[] { '\x0501' }),                      /* 0500; 0501; Case map */
            new CharMap('\x0502', new char[] { '\x0503' }),                      /* 0502; 0503; Case map */
            new CharMap('\x0504', new char[] { '\x0505' }),                      /* 0504; 0505; Case map */
            new CharMap('\x0506', new char[] { '\x0507' }),                      /* 0506; 0507; Case map */
            new CharMap('\x0508', new char[] { '\x0509' }),                      /* 0508; 0509; Case map */
            new CharMap('\x050A', new char[] { '\x050B' }),                      /* 050A; 050B; Case map */
            new CharMap('\x050C', new char[] { '\x050D' }),                      /* 050C; 050D; Case map */
            new CharMap('\x050E', new char[] { '\x050F' }),                      /* 050E; 050F; Case map */
            new CharMap('\x0531', new char[] { '\x0561' }),                      /* 0531; 0561; Case map */
            new CharMap('\x0532', new char[] { '\x0562' }),                      /* 0532; 0562; Case map */
            new CharMap('\x0533', new char[] { '\x0563' }),                      /* 0533; 0563; Case map */
            new CharMap('\x0534', new char[] { '\x0564' }),                      /* 0534; 0564; Case map */
            new CharMap('\x0535', new char[] { '\x0565' }),                      /* 0535; 0565; Case map */
            new CharMap('\x0536', new char[] { '\x0566' }),                      /* 0536; 0566; Case map */
            new CharMap('\x0537', new char[] { '\x0567' }),                      /* 0537; 0567; Case map */
            new CharMap('\x0538', new char[] { '\x0568' }),                      /* 0538; 0568; Case map */
            new CharMap('\x0539', new char[] { '\x0569' }),                      /* 0539; 0569; Case map */
            new CharMap('\x053A', new char[] { '\x056A' }),                      /* 053A; 056A; Case map */
            new CharMap('\x053B', new char[] { '\x056B' }),                      /* 053B; 056B; Case map */
            new CharMap('\x053C', new char[] { '\x056C' }),                      /* 053C; 056C; Case map */
            new CharMap('\x053D', new char[] { '\x056D' }),                      /* 053D; 056D; Case map */
            new CharMap('\x053E', new char[] { '\x056E' }),                      /* 053E; 056E; Case map */
            new CharMap('\x053F', new char[] { '\x056F' }),                      /* 053F; 056F; Case map */
            new CharMap('\x0540', new char[] { '\x0570' }),                      /* 0540; 0570; Case map */
            new CharMap('\x0541', new char[] { '\x0571' }),                      /* 0541; 0571; Case map */
            new CharMap('\x0542', new char[] { '\x0572' }),                      /* 0542; 0572; Case map */
            new CharMap('\x0543', new char[] { '\x0573' }),                      /* 0543; 0573; Case map */
            new CharMap('\x0544', new char[] { '\x0574' }),                      /* 0544; 0574; Case map */
            new CharMap('\x0545', new char[] { '\x0575' }),                      /* 0545; 0575; Case map */
            new CharMap('\x0546', new char[] { '\x0576' }),                      /* 0546; 0576; Case map */
            new CharMap('\x0547', new char[] { '\x0577' }),                      /* 0547; 0577; Case map */
            new CharMap('\x0548', new char[] { '\x0578' }),                      /* 0548; 0578; Case map */
            new CharMap('\x0549', new char[] { '\x0579' }),                      /* 0549; 0579; Case map */
            new CharMap('\x054A', new char[] { '\x057A' }),                      /* 054A; 057A; Case map */
            new CharMap('\x054B', new char[] { '\x057B' }),                      /* 054B; 057B; Case map */
            new CharMap('\x054C', new char[] { '\x057C' }),                      /* 054C; 057C; Case map */
            new CharMap('\x054D', new char[] { '\x057D' }),                      /* 054D; 057D; Case map */
            new CharMap('\x054E', new char[] { '\x057E' }),                      /* 054E; 057E; Case map */
            new CharMap('\x054F', new char[] { '\x057F' }),                      /* 054F; 057F; Case map */
            new CharMap('\x0550', new char[] { '\x0580' }),                      /* 0550; 0580; Case map */
            new CharMap('\x0551', new char[] { '\x0581' }),                      /* 0551; 0581; Case map */
            new CharMap('\x0552', new char[] { '\x0582' }),                      /* 0552; 0582; Case map */
            new CharMap('\x0553', new char[] { '\x0583' }),                      /* 0553; 0583; Case map */
            new CharMap('\x0554', new char[] { '\x0584' }),                      /* 0554; 0584; Case map */
            new CharMap('\x0555', new char[] { '\x0585' }),                      /* 0555; 0585; Case map */
            new CharMap('\x0556', new char[] { '\x0586' }),                      /* 0556; 0586; Case map */
            new CharMap('\x0587', new char[] { '\x0565',                    /* 0587; 0565 0582; Case map */
                   '\x0582' }),
            new CharMap('\x1E00', new char[] { '\x1E01' }),                      /* 1E00; 1E01; Case map */
            new CharMap('\x1E02', new char[] { '\x1E03' }),                      /* 1E02; 1E03; Case map */
            new CharMap('\x1E04', new char[] { '\x1E05' }),                      /* 1E04; 1E05; Case map */
            new CharMap('\x1E06', new char[] { '\x1E07' }),                      /* 1E06; 1E07; Case map */
            new CharMap('\x1E08', new char[] { '\x1E09' }),                      /* 1E08; 1E09; Case map */
            new CharMap('\x1E0A', new char[] { '\x1E0B' }),                      /* 1E0A; 1E0B; Case map */
            new CharMap('\x1E0C', new char[] { '\x1E0D' }),                      /* 1E0C; 1E0D; Case map */
            new CharMap('\x1E0E', new char[] { '\x1E0F' }),                      /* 1E0E; 1E0F; Case map */
            new CharMap('\x1E10', new char[] { '\x1E11' }),                      /* 1E10; 1E11; Case map */
            new CharMap('\x1E12', new char[] { '\x1E13' }),                      /* 1E12; 1E13; Case map */
            new CharMap('\x1E14', new char[] { '\x1E15' }),                      /* 1E14; 1E15; Case map */
            new CharMap('\x1E16', new char[] { '\x1E17' }),                      /* 1E16; 1E17; Case map */
            new CharMap('\x1E18', new char[] { '\x1E19' }),                      /* 1E18; 1E19; Case map */
            new CharMap('\x1E1A', new char[] { '\x1E1B' }),                      /* 1E1A; 1E1B; Case map */
            new CharMap('\x1E1C', new char[] { '\x1E1D' }),                      /* 1E1C; 1E1D; Case map */
            new CharMap('\x1E1E', new char[] { '\x1E1F' }),                      /* 1E1E; 1E1F; Case map */
            new CharMap('\x1E20', new char[] { '\x1E21' }),                      /* 1E20; 1E21; Case map */
            new CharMap('\x1E22', new char[] { '\x1E23' }),                      /* 1E22; 1E23; Case map */
            new CharMap('\x1E24', new char[] { '\x1E25' }),                      /* 1E24; 1E25; Case map */
            new CharMap('\x1E26', new char[] { '\x1E27' }),                      /* 1E26; 1E27; Case map */
            new CharMap('\x1E28', new char[] { '\x1E29' }),                      /* 1E28; 1E29; Case map */
            new CharMap('\x1E2A', new char[] { '\x1E2B' }),                      /* 1E2A; 1E2B; Case map */
            new CharMap('\x1E2C', new char[] { '\x1E2D' }),                      /* 1E2C; 1E2D; Case map */
            new CharMap('\x1E2E', new char[] { '\x1E2F' }),                      /* 1E2E; 1E2F; Case map */
            new CharMap('\x1E30', new char[] { '\x1E31' }),                      /* 1E30; 1E31; Case map */
            new CharMap('\x1E32', new char[] { '\x1E33' }),                      /* 1E32; 1E33; Case map */
            new CharMap('\x1E34', new char[] { '\x1E35' }),                      /* 1E34; 1E35; Case map */
            new CharMap('\x1E36', new char[] { '\x1E37' }),                      /* 1E36; 1E37; Case map */
            new CharMap('\x1E38', new char[] { '\x1E39' }),                      /* 1E38; 1E39; Case map */
            new CharMap('\x1E3A', new char[] { '\x1E3B' }),                      /* 1E3A; 1E3B; Case map */
            new CharMap('\x1E3C', new char[] { '\x1E3D' }),                      /* 1E3C; 1E3D; Case map */
            new CharMap('\x1E3E', new char[] { '\x1E3F' }),                      /* 1E3E; 1E3F; Case map */
            new CharMap('\x1E40', new char[] { '\x1E41' }),                      /* 1E40; 1E41; Case map */
            new CharMap('\x1E42', new char[] { '\x1E43' }),                      /* 1E42; 1E43; Case map */
            new CharMap('\x1E44', new char[] { '\x1E45' }),                      /* 1E44; 1E45; Case map */
            new CharMap('\x1E46', new char[] { '\x1E47' }),                      /* 1E46; 1E47; Case map */
            new CharMap('\x1E48', new char[] { '\x1E49' }),                      /* 1E48; 1E49; Case map */
            new CharMap('\x1E4A', new char[] { '\x1E4B' }),                      /* 1E4A; 1E4B; Case map */
            new CharMap('\x1E4C', new char[] { '\x1E4D' }),                      /* 1E4C; 1E4D; Case map */
            new CharMap('\x1E4E', new char[] { '\x1E4F' }),                      /* 1E4E; 1E4F; Case map */
            new CharMap('\x1E50', new char[] { '\x1E51' }),                      /* 1E50; 1E51; Case map */
            new CharMap('\x1E52', new char[] { '\x1E53' }),                      /* 1E52; 1E53; Case map */
            new CharMap('\x1E54', new char[] { '\x1E55' }),                      /* 1E54; 1E55; Case map */
            new CharMap('\x1E56', new char[] { '\x1E57' }),                      /* 1E56; 1E57; Case map */
            new CharMap('\x1E58', new char[] { '\x1E59' }),                      /* 1E58; 1E59; Case map */
            new CharMap('\x1E5A', new char[] { '\x1E5B' }),                      /* 1E5A; 1E5B; Case map */
            new CharMap('\x1E5C', new char[] { '\x1E5D' }),                      /* 1E5C; 1E5D; Case map */
            new CharMap('\x1E5E', new char[] { '\x1E5F' }),                      /* 1E5E; 1E5F; Case map */
            new CharMap('\x1E60', new char[] { '\x1E61' }),                      /* 1E60; 1E61; Case map */
            new CharMap('\x1E62', new char[] { '\x1E63' }),                      /* 1E62; 1E63; Case map */
            new CharMap('\x1E64', new char[] { '\x1E65' }),                      /* 1E64; 1E65; Case map */
            new CharMap('\x1E66', new char[] { '\x1E67' }),                      /* 1E66; 1E67; Case map */
            new CharMap('\x1E68', new char[] { '\x1E69' }),                      /* 1E68; 1E69; Case map */
            new CharMap('\x1E6A', new char[] { '\x1E6B' }),                      /* 1E6A; 1E6B; Case map */
            new CharMap('\x1E6C', new char[] { '\x1E6D' }),                      /* 1E6C; 1E6D; Case map */
            new CharMap('\x1E6E', new char[] { '\x1E6F' }),                      /* 1E6E; 1E6F; Case map */
            new CharMap('\x1E70', new char[] { '\x1E71' }),                      /* 1E70; 1E71; Case map */
            new CharMap('\x1E72', new char[] { '\x1E73' }),                      /* 1E72; 1E73; Case map */
            new CharMap('\x1E74', new char[] { '\x1E75' }),                      /* 1E74; 1E75; Case map */
            new CharMap('\x1E76', new char[] { '\x1E77' }),                      /* 1E76; 1E77; Case map */
            new CharMap('\x1E78', new char[] { '\x1E79' }),                      /* 1E78; 1E79; Case map */
            new CharMap('\x1E7A', new char[] { '\x1E7B' }),                      /* 1E7A; 1E7B; Case map */
            new CharMap('\x1E7C', new char[] { '\x1E7D' }),                      /* 1E7C; 1E7D; Case map */
            new CharMap('\x1E7E', new char[] { '\x1E7F' }),                      /* 1E7E; 1E7F; Case map */
            new CharMap('\x1E80', new char[] { '\x1E81' }),                      /* 1E80; 1E81; Case map */
            new CharMap('\x1E82', new char[] { '\x1E83' }),                      /* 1E82; 1E83; Case map */
            new CharMap('\x1E84', new char[] { '\x1E85' }),                      /* 1E84; 1E85; Case map */
            new CharMap('\x1E86', new char[] { '\x1E87' }),                      /* 1E86; 1E87; Case map */
            new CharMap('\x1E88', new char[] { '\x1E89' }),                      /* 1E88; 1E89; Case map */
            new CharMap('\x1E8A', new char[] { '\x1E8B' }),                      /* 1E8A; 1E8B; Case map */
            new CharMap('\x1E8C', new char[] { '\x1E8D' }),                      /* 1E8C; 1E8D; Case map */
            new CharMap('\x1E8E', new char[] { '\x1E8F' }),                      /* 1E8E; 1E8F; Case map */
            new CharMap('\x1E90', new char[] { '\x1E91' }),                      /* 1E90; 1E91; Case map */
            new CharMap('\x1E92', new char[] { '\x1E93' }),                      /* 1E92; 1E93; Case map */
            new CharMap('\x1E94', new char[] { '\x1E95' }),                      /* 1E94; 1E95; Case map */
            new CharMap('\x1E96', new char[] { '\x0068',                    /* 1E96; 0068 0331; Case map */
                   '\x0331' }),
            new CharMap('\x1E97', new char[] { '\x0074',                    /* 1E97; 0074 0308; Case map */
                   '\x0308' }),
            new CharMap('\x1E98', new char[] { '\x0077',                    /* 1E98; 0077 030A; Case map */
                   '\x030A' }),
            new CharMap('\x1E99', new char[] { '\x0079',                    /* 1E99; 0079 030A; Case map */
                   '\x030A' }),
            new CharMap('\x1E9A', new char[] { '\x0061',                    /* 1E9A; 0061 02BE; Case map */
                   '\x02BE' }),
            new CharMap('\x1E9B', new char[] { '\x1E61' }),                      /* 1E9B; 1E61; Case map */
            new CharMap('\x1EA0', new char[] { '\x1EA1' }),                      /* 1EA0; 1EA1; Case map */
            new CharMap('\x1EA2', new char[] { '\x1EA3' }),                      /* 1EA2; 1EA3; Case map */
            new CharMap('\x1EA4', new char[] { '\x1EA5' }),                      /* 1EA4; 1EA5; Case map */
            new CharMap('\x1EA6', new char[] { '\x1EA7' }),                      /* 1EA6; 1EA7; Case map */
            new CharMap('\x1EA8', new char[] { '\x1EA9' }),                      /* 1EA8; 1EA9; Case map */
            new CharMap('\x1EAA', new char[] { '\x1EAB' }),                      /* 1EAA; 1EAB; Case map */
            new CharMap('\x1EAC', new char[] { '\x1EAD' }),                      /* 1EAC; 1EAD; Case map */
            new CharMap('\x1EAE', new char[] { '\x1EAF' }),                      /* 1EAE; 1EAF; Case map */
            new CharMap('\x1EB0', new char[] { '\x1EB1' }),                      /* 1EB0; 1EB1; Case map */
            new CharMap('\x1EB2', new char[] { '\x1EB3' }),                      /* 1EB2; 1EB3; Case map */
            new CharMap('\x1EB4', new char[] { '\x1EB5' }),                      /* 1EB4; 1EB5; Case map */
            new CharMap('\x1EB6', new char[] { '\x1EB7' }),                      /* 1EB6; 1EB7; Case map */
            new CharMap('\x1EB8', new char[] { '\x1EB9' }),                      /* 1EB8; 1EB9; Case map */
            new CharMap('\x1EBA', new char[] { '\x1EBB' }),                      /* 1EBA; 1EBB; Case map */
            new CharMap('\x1EBC', new char[] { '\x1EBD' }),                      /* 1EBC; 1EBD; Case map */
            new CharMap('\x1EBE', new char[] { '\x1EBF' }),                      /* 1EBE; 1EBF; Case map */
            new CharMap('\x1EC0', new char[] { '\x1EC1' }),                      /* 1EC0; 1EC1; Case map */
            new CharMap('\x1EC2', new char[] { '\x1EC3' }),                      /* 1EC2; 1EC3; Case map */
            new CharMap('\x1EC4', new char[] { '\x1EC5' }),                      /* 1EC4; 1EC5; Case map */
            new CharMap('\x1EC6', new char[] { '\x1EC7' }),                      /* 1EC6; 1EC7; Case map */
            new CharMap('\x1EC8', new char[] { '\x1EC9' }),                      /* 1EC8; 1EC9; Case map */
            new CharMap('\x1ECA', new char[] { '\x1ECB' }),                      /* 1ECA; 1ECB; Case map */
            new CharMap('\x1ECC', new char[] { '\x1ECD' }),                      /* 1ECC; 1ECD; Case map */
            new CharMap('\x1ECE', new char[] { '\x1ECF' }),                      /* 1ECE; 1ECF; Case map */
            new CharMap('\x1ED0', new char[] { '\x1ED1' }),                      /* 1ED0; 1ED1; Case map */
            new CharMap('\x1ED2', new char[] { '\x1ED3' }),                      /* 1ED2; 1ED3; Case map */
            new CharMap('\x1ED4', new char[] { '\x1ED5' }),                      /* 1ED4; 1ED5; Case map */
            new CharMap('\x1ED6', new char[] { '\x1ED7' }),                      /* 1ED6; 1ED7; Case map */
            new CharMap('\x1ED8', new char[] { '\x1ED9' }),                      /* 1ED8; 1ED9; Case map */
            new CharMap('\x1EDA', new char[] { '\x1EDB' }),                      /* 1EDA; 1EDB; Case map */
            new CharMap('\x1EDC', new char[] { '\x1EDD' }),                      /* 1EDC; 1EDD; Case map */
            new CharMap('\x1EDE', new char[] { '\x1EDF' }),                      /* 1EDE; 1EDF; Case map */
            new CharMap('\x1EE0', new char[] { '\x1EE1' }),                      /* 1EE0; 1EE1; Case map */
            new CharMap('\x1EE2', new char[] { '\x1EE3' }),                      /* 1EE2; 1EE3; Case map */
            new CharMap('\x1EE4', new char[] { '\x1EE5' }),                      /* 1EE4; 1EE5; Case map */
            new CharMap('\x1EE6', new char[] { '\x1EE7' }),                      /* 1EE6; 1EE7; Case map */
            new CharMap('\x1EE8', new char[] { '\x1EE9' }),                      /* 1EE8; 1EE9; Case map */
            new CharMap('\x1EEA', new char[] { '\x1EEB' }),                      /* 1EEA; 1EEB; Case map */
            new CharMap('\x1EEC', new char[] { '\x1EED' }),                      /* 1EEC; 1EED; Case map */
            new CharMap('\x1EEE', new char[] { '\x1EEF' }),                      /* 1EEE; 1EEF; Case map */
            new CharMap('\x1EF0', new char[] { '\x1EF1' }),                      /* 1EF0; 1EF1; Case map */
            new CharMap('\x1EF2', new char[] { '\x1EF3' }),                      /* 1EF2; 1EF3; Case map */
            new CharMap('\x1EF4', new char[] { '\x1EF5' }),                      /* 1EF4; 1EF5; Case map */
            new CharMap('\x1EF6', new char[] { '\x1EF7' }),                      /* 1EF6; 1EF7; Case map */
            new CharMap('\x1EF8', new char[] { '\x1EF9' }),                      /* 1EF8; 1EF9; Case map */
            new CharMap('\x1F08', new char[] { '\x1F00' }),                      /* 1F08; 1F00; Case map */
            new CharMap('\x1F09', new char[] { '\x1F01' }),                      /* 1F09; 1F01; Case map */
            new CharMap('\x1F0A', new char[] { '\x1F02' }),                      /* 1F0A; 1F02; Case map */
            new CharMap('\x1F0B', new char[] { '\x1F03' }),                      /* 1F0B; 1F03; Case map */
            new CharMap('\x1F0C', new char[] { '\x1F04' }),                      /* 1F0C; 1F04; Case map */
            new CharMap('\x1F0D', new char[] { '\x1F05' }),                      /* 1F0D; 1F05; Case map */
            new CharMap('\x1F0E', new char[] { '\x1F06' }),                      /* 1F0E; 1F06; Case map */
            new CharMap('\x1F0F', new char[] { '\x1F07' }),                      /* 1F0F; 1F07; Case map */
            new CharMap('\x1F18', new char[] { '\x1F10' }),                      /* 1F18; 1F10; Case map */
            new CharMap('\x1F19', new char[] { '\x1F11' }),                      /* 1F19; 1F11; Case map */
            new CharMap('\x1F1A', new char[] { '\x1F12' }),                      /* 1F1A; 1F12; Case map */
            new CharMap('\x1F1B', new char[] { '\x1F13' }),                      /* 1F1B; 1F13; Case map */
            new CharMap('\x1F1C', new char[] { '\x1F14' }),                      /* 1F1C; 1F14; Case map */
            new CharMap('\x1F1D', new char[] { '\x1F15' }),                      /* 1F1D; 1F15; Case map */
            new CharMap('\x1F28', new char[] { '\x1F20' }),                      /* 1F28; 1F20; Case map */
            new CharMap('\x1F29', new char[] { '\x1F21' }),                      /* 1F29; 1F21; Case map */
            new CharMap('\x1F2A', new char[] { '\x1F22' }),                      /* 1F2A; 1F22; Case map */
            new CharMap('\x1F2B', new char[] { '\x1F23' }),                      /* 1F2B; 1F23; Case map */
            new CharMap('\x1F2C', new char[] { '\x1F24' }),                      /* 1F2C; 1F24; Case map */
            new CharMap('\x1F2D', new char[] { '\x1F25' }),                      /* 1F2D; 1F25; Case map */
            new CharMap('\x1F2E', new char[] { '\x1F26' }),                      /* 1F2E; 1F26; Case map */
            new CharMap('\x1F2F', new char[] { '\x1F27' }),                      /* 1F2F; 1F27; Case map */
            new CharMap('\x1F38', new char[] { '\x1F30' }),                      /* 1F38; 1F30; Case map */
            new CharMap('\x1F39', new char[] { '\x1F31' }),                      /* 1F39; 1F31; Case map */
            new CharMap('\x1F3A', new char[] { '\x1F32' }),                      /* 1F3A; 1F32; Case map */
            new CharMap('\x1F3B', new char[] { '\x1F33' }),                      /* 1F3B; 1F33; Case map */
            new CharMap('\x1F3C', new char[] { '\x1F34' }),                      /* 1F3C; 1F34; Case map */
            new CharMap('\x1F3D', new char[] { '\x1F35' }),                      /* 1F3D; 1F35; Case map */
            new CharMap('\x1F3E', new char[] { '\x1F36' }),                      /* 1F3E; 1F36; Case map */
            new CharMap('\x1F3F', new char[] { '\x1F37' }),                      /* 1F3F; 1F37; Case map */
            new CharMap('\x1F48', new char[] { '\x1F40' }),                      /* 1F48; 1F40; Case map */
            new CharMap('\x1F49', new char[] { '\x1F41' }),                      /* 1F49; 1F41; Case map */
            new CharMap('\x1F4A', new char[] { '\x1F42' }),                      /* 1F4A; 1F42; Case map */
            new CharMap('\x1F4B', new char[] { '\x1F43' }),                      /* 1F4B; 1F43; Case map */
            new CharMap('\x1F4C', new char[] { '\x1F44' }),                      /* 1F4C; 1F44; Case map */
            new CharMap('\x1F4D', new char[] { '\x1F45' }),                      /* 1F4D; 1F45; Case map */
            new CharMap('\x1F50', new char[] { '\x03C5',                    /* 1F50; 03C5 0313; Case map */
                   '\x0313' }),
            new CharMap('\x1F52', new char[] { '\x03C5',               /* 1F52; 03C5 0313 0300; Case map */
                   '\x0313', '\x0300' }),
            new CharMap('\x1F54', new char[] { '\x03C5',               /* 1F54; 03C5 0313 0301; Case map */
                   '\x0313', '\x0301' }),
            new CharMap('\x1F56', new char[] { '\x03C5',               /* 1F56; 03C5 0313 0342; Case map */
                   '\x0313', '\x0342' }),
            new CharMap('\x1F59', new char[] { '\x1F51' }),                      /* 1F59; 1F51; Case map */
            new CharMap('\x1F5B', new char[] { '\x1F53' }),                      /* 1F5B; 1F53; Case map */
            new CharMap('\x1F5D', new char[] { '\x1F55' }),                      /* 1F5D; 1F55; Case map */
            new CharMap('\x1F5F', new char[] { '\x1F57' }),                      /* 1F5F; 1F57; Case map */
            new CharMap('\x1F68', new char[] { '\x1F60' }),                      /* 1F68; 1F60; Case map */
            new CharMap('\x1F69', new char[] { '\x1F61' }),                      /* 1F69; 1F61; Case map */
            new CharMap('\x1F6A', new char[] { '\x1F62' }),                      /* 1F6A; 1F62; Case map */
            new CharMap('\x1F6B', new char[] { '\x1F63' }),                      /* 1F6B; 1F63; Case map */
            new CharMap('\x1F6C', new char[] { '\x1F64' }),                      /* 1F6C; 1F64; Case map */
            new CharMap('\x1F6D', new char[] { '\x1F65' }),                      /* 1F6D; 1F65; Case map */
            new CharMap('\x1F6E', new char[] { '\x1F66' }),                      /* 1F6E; 1F66; Case map */
            new CharMap('\x1F6F', new char[] { '\x1F67' }),                      /* 1F6F; 1F67; Case map */
            new CharMap('\x1F80', new char[] { '\x1F00',                    /* 1F80; 1F00 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F81', new char[] { '\x1F01',                    /* 1F81; 1F01 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F82', new char[] { '\x1F02',                    /* 1F82; 1F02 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F83', new char[] { '\x1F03',                    /* 1F83; 1F03 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F84', new char[] { '\x1F04',                    /* 1F84; 1F04 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F85', new char[] { '\x1F05',                    /* 1F85; 1F05 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F86', new char[] { '\x1F06',                    /* 1F86; 1F06 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F87', new char[] { '\x1F07',                    /* 1F87; 1F07 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F88', new char[] { '\x1F00',                    /* 1F88; 1F00 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F89', new char[] { '\x1F01',                    /* 1F89; 1F01 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8A', new char[] { '\x1F02',                    /* 1F8A; 1F02 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8B', new char[] { '\x1F03',                    /* 1F8B; 1F03 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8C', new char[] { '\x1F04',                    /* 1F8C; 1F04 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8D', new char[] { '\x1F05',                    /* 1F8D; 1F05 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8E', new char[] { '\x1F06',                    /* 1F8E; 1F06 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F8F', new char[] { '\x1F07',                    /* 1F8F; 1F07 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F90', new char[] { '\x1F20',                    /* 1F90; 1F20 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F91', new char[] { '\x1F21',                    /* 1F91; 1F21 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F92', new char[] { '\x1F22',                    /* 1F92; 1F22 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F93', new char[] { '\x1F23',                    /* 1F93; 1F23 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F94', new char[] { '\x1F24',                    /* 1F94; 1F24 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F95', new char[] { '\x1F25',                    /* 1F95; 1F25 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F96', new char[] { '\x1F26',                    /* 1F96; 1F26 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F97', new char[] { '\x1F27',                    /* 1F97; 1F27 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F98', new char[] { '\x1F20',                    /* 1F98; 1F20 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F99', new char[] { '\x1F21',                    /* 1F99; 1F21 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9A', new char[] { '\x1F22',                    /* 1F9A; 1F22 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9B', new char[] { '\x1F23',                    /* 1F9B; 1F23 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9C', new char[] { '\x1F24',                    /* 1F9C; 1F24 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9D', new char[] { '\x1F25',                    /* 1F9D; 1F25 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9E', new char[] { '\x1F26',                    /* 1F9E; 1F26 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1F9F', new char[] { '\x1F27',                    /* 1F9F; 1F27 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA0', new char[] { '\x1F60',                    /* 1FA0; 1F60 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA1', new char[] { '\x1F61',                    /* 1FA1; 1F61 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA2', new char[] { '\x1F62',                    /* 1FA2; 1F62 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA3', new char[] { '\x1F63',                    /* 1FA3; 1F63 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA4', new char[] { '\x1F64',                    /* 1FA4; 1F64 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA5', new char[] { '\x1F65',                    /* 1FA5; 1F65 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA6', new char[] { '\x1F66',                    /* 1FA6; 1F66 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA7', new char[] { '\x1F67',                    /* 1FA7; 1F67 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA8', new char[] { '\x1F60',                    /* 1FA8; 1F60 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FA9', new char[] { '\x1F61',                    /* 1FA9; 1F61 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAA', new char[] { '\x1F62',                    /* 1FAA; 1F62 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAB', new char[] { '\x1F63',                    /* 1FAB; 1F63 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAC', new char[] { '\x1F64',                    /* 1FAC; 1F64 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAD', new char[] { '\x1F65',                    /* 1FAD; 1F65 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAE', new char[] { '\x1F66',                    /* 1FAE; 1F66 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FAF', new char[] { '\x1F67',                    /* 1FAF; 1F67 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB2', new char[] { '\x1F70',                    /* 1FB2; 1F70 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB3', new char[] { '\x03B1',                    /* 1FB3; 03B1 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB4', new char[] { '\x03AC',                    /* 1FB4; 03AC 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FB6', new char[] { '\x03B1',                    /* 1FB6; 03B1 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FB7', new char[] { '\x03B1',               /* 1FB7; 03B1 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FB8', new char[] { '\x1FB0' }),                      /* 1FB8; 1FB0; Case map */
            new CharMap('\x1FB9', new char[] { '\x1FB1' }),                      /* 1FB9; 1FB1; Case map */
            new CharMap('\x1FBA', new char[] { '\x1F70' }),                      /* 1FBA; 1F70; Case map */
            new CharMap('\x1FBB', new char[] { '\x1F71' }),                      /* 1FBB; 1F71; Case map */
            new CharMap('\x1FBC', new char[] { '\x03B1',                    /* 1FBC; 03B1 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FBE', new char[] { '\x03B9' }),                      /* 1FBE; 03B9; Case map */
            new CharMap('\x1FC2', new char[] { '\x1F74',                    /* 1FC2; 1F74 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC3', new char[] { '\x03B7',                    /* 1FC3; 03B7 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC4', new char[] { '\x03AE',                    /* 1FC4; 03AE 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FC6', new char[] { '\x03B7',                    /* 1FC6; 03B7 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FC7', new char[] { '\x03B7',               /* 1FC7; 03B7 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FC8', new char[] { '\x1F72' }),                      /* 1FC8; 1F72; Case map */
            new CharMap('\x1FC9', new char[] { '\x1F73' }),                      /* 1FC9; 1F73; Case map */
            new CharMap('\x1FCA', new char[] { '\x1F74' }),                      /* 1FCA; 1F74; Case map */
            new CharMap('\x1FCB', new char[] { '\x1F75' }),                      /* 1FCB; 1F75; Case map */
            new CharMap('\x1FCC', new char[] { '\x03B7',                    /* 1FCC; 03B7 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FD2', new char[] { '\x03B9',               /* 1FD2; 03B9 0308 0300; Case map */
                   '\x0308', '\x0300' }),
            new CharMap('\x1FD3', new char[] { '\x03B9',               /* 1FD3; 03B9 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x1FD6', new char[] { '\x03B9',                    /* 1FD6; 03B9 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FD7', new char[] { '\x03B9',               /* 1FD7; 03B9 0308 0342; Case map */
                   '\x0308', '\x0342' }),
            new CharMap('\x1FD8', new char[] { '\x1FD0' }),                      /* 1FD8; 1FD0; Case map */
            new CharMap('\x1FD9', new char[] { '\x1FD1' }),                      /* 1FD9; 1FD1; Case map */
            new CharMap('\x1FDA', new char[] { '\x1F76' }),                      /* 1FDA; 1F76; Case map */
            new CharMap('\x1FDB', new char[] { '\x1F77' }),                      /* 1FDB; 1F77; Case map */
            new CharMap('\x1FE2', new char[] { '\x03C5',               /* 1FE2; 03C5 0308 0300; Case map */
                   '\x0308', '\x0300' }),
            new CharMap('\x1FE3', new char[] { '\x03C5',               /* 1FE3; 03C5 0308 0301; Case map */
                   '\x0308', '\x0301' }),
            new CharMap('\x1FE4', new char[] { '\x03C1',                    /* 1FE4; 03C1 0313; Case map */
                   '\x0313' }),
            new CharMap('\x1FE6', new char[] { '\x03C5',                    /* 1FE6; 03C5 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FE7', new char[] { '\x03C5',               /* 1FE7; 03C5 0308 0342; Case map */
                   '\x0308', '\x0342' }),
            new CharMap('\x1FE8', new char[] { '\x1FE0' }),                      /* 1FE8; 1FE0; Case map */
            new CharMap('\x1FE9', new char[] { '\x1FE1' }),                      /* 1FE9; 1FE1; Case map */
            new CharMap('\x1FEA', new char[] { '\x1F7A' }),                      /* 1FEA; 1F7A; Case map */
            new CharMap('\x1FEB', new char[] { '\x1F7B' }),                      /* 1FEB; 1F7B; Case map */
            new CharMap('\x1FEC', new char[] { '\x1FE5' }),                      /* 1FEC; 1FE5; Case map */
            new CharMap('\x1FF2', new char[] { '\x1F7C',                    /* 1FF2; 1F7C 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF3', new char[] { '\x03C9',                    /* 1FF3; 03C9 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF4', new char[] { '\x03CE',                    /* 1FF4; 03CE 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x1FF6', new char[] { '\x03C9',                    /* 1FF6; 03C9 0342; Case map */
                   '\x0342' }),
            new CharMap('\x1FF7', new char[] { '\x03C9',               /* 1FF7; 03C9 0342 03B9; Case map */
                   '\x0342', '\x03B9' }),
            new CharMap('\x1FF8', new char[] { '\x1F78' }),                      /* 1FF8; 1F78; Case map */
            new CharMap('\x1FF9', new char[] { '\x1F79' }),                      /* 1FF9; 1F79; Case map */
            new CharMap('\x1FFA', new char[] { '\x1F7C' }),                      /* 1FFA; 1F7C; Case map */
            new CharMap('\x1FFB', new char[] { '\x1F7D' }),                      /* 1FFB; 1F7D; Case map */
            new CharMap('\x1FFC', new char[] { '\x03C9',                    /* 1FFC; 03C9 03B9; Case map */
                   '\x03B9' }),
            new CharMap('\x2126', new char[] { '\x03C9' }),                      /* 2126; 03C9; Case map */
            new CharMap('\x212A', new char[] { '\x006B' }),                      /* 212A; 006B; Case map */
            new CharMap('\x212B', new char[] { '\x00E5' }),                      /* 212B; 00E5; Case map */
            new CharMap('\x2160', new char[] { '\x2170' }),                      /* 2160; 2170; Case map */
            new CharMap('\x2161', new char[] { '\x2171' }),                      /* 2161; 2171; Case map */
            new CharMap('\x2162', new char[] { '\x2172' }),                      /* 2162; 2172; Case map */
            new CharMap('\x2163', new char[] { '\x2173' }),                      /* 2163; 2173; Case map */
            new CharMap('\x2164', new char[] { '\x2174' }),                      /* 2164; 2174; Case map */
            new CharMap('\x2165', new char[] { '\x2175' }),                      /* 2165; 2175; Case map */
            new CharMap('\x2166', new char[] { '\x2176' }),                      /* 2166; 2176; Case map */
            new CharMap('\x2167', new char[] { '\x2177' }),                      /* 2167; 2177; Case map */
            new CharMap('\x2168', new char[] { '\x2178' }),                      /* 2168; 2178; Case map */
            new CharMap('\x2169', new char[] { '\x2179' }),                      /* 2169; 2179; Case map */
            new CharMap('\x216A', new char[] { '\x217A' }),                      /* 216A; 217A; Case map */
            new CharMap('\x216B', new char[] { '\x217B' }),                      /* 216B; 217B; Case map */
            new CharMap('\x216C', new char[] { '\x217C' }),                      /* 216C; 217C; Case map */
            new CharMap('\x216D', new char[] { '\x217D' }),                      /* 216D; 217D; Case map */
            new CharMap('\x216E', new char[] { '\x217E' }),                      /* 216E; 217E; Case map */
            new CharMap('\x216F', new char[] { '\x217F' }),                      /* 216F; 217F; Case map */
            new CharMap('\x24B6', new char[] { '\x24D0' }),                      /* 24B6; 24D0; Case map */
            new CharMap('\x24B7', new char[] { '\x24D1' }),                      /* 24B7; 24D1; Case map */
            new CharMap('\x24B8', new char[] { '\x24D2' }),                      /* 24B8; 24D2; Case map */
            new CharMap('\x24B9', new char[] { '\x24D3' }),                      /* 24B9; 24D3; Case map */
            new CharMap('\x24BA', new char[] { '\x24D4' }),                      /* 24BA; 24D4; Case map */
            new CharMap('\x24BB', new char[] { '\x24D5' }),                      /* 24BB; 24D5; Case map */
            new CharMap('\x24BC', new char[] { '\x24D6' }),                      /* 24BC; 24D6; Case map */
            new CharMap('\x24BD', new char[] { '\x24D7' }),                      /* 24BD; 24D7; Case map */
            new CharMap('\x24BE', new char[] { '\x24D8' }),                      /* 24BE; 24D8; Case map */
            new CharMap('\x24BF', new char[] { '\x24D9' }),                      /* 24BF; 24D9; Case map */
            new CharMap('\x24C0', new char[] { '\x24DA' }),                      /* 24C0; 24DA; Case map */
            new CharMap('\x24C1', new char[] { '\x24DB' }),                      /* 24C1; 24DB; Case map */
            new CharMap('\x24C2', new char[] { '\x24DC' }),                      /* 24C2; 24DC; Case map */
            new CharMap('\x24C3', new char[] { '\x24DD' }),                      /* 24C3; 24DD; Case map */
            new CharMap('\x24C4', new char[] { '\x24DE' }),                      /* 24C4; 24DE; Case map */
            new CharMap('\x24C5', new char[] { '\x24DF' }),                      /* 24C5; 24DF; Case map */
            new CharMap('\x24C6', new char[] { '\x24E0' }),                      /* 24C6; 24E0; Case map */
            new CharMap('\x24C7', new char[] { '\x24E1' }),                      /* 24C7; 24E1; Case map */
            new CharMap('\x24C8', new char[] { '\x24E2' }),                      /* 24C8; 24E2; Case map */
            new CharMap('\x24C9', new char[] { '\x24E3' }),                      /* 24C9; 24E3; Case map */
            new CharMap('\x24CA', new char[] { '\x24E4' }),                      /* 24CA; 24E4; Case map */
            new CharMap('\x24CB', new char[] { '\x24E5' }),                      /* 24CB; 24E5; Case map */
            new CharMap('\x24CC', new char[] { '\x24E6' }),                      /* 24CC; 24E6; Case map */
            new CharMap('\x24CD', new char[] { '\x24E7' }),                      /* 24CD; 24E7; Case map */
            new CharMap('\x24CE', new char[] { '\x24E8' }),                      /* 24CE; 24E8; Case map */
            new CharMap('\x24CF', new char[] { '\x24E9' }),                      /* 24CF; 24E9; Case map */
            new CharMap('\xFB00', new char[] { '\x0066',                    /* FB00; 0066 0066; Case map */
                   '\x0066' }),
            new CharMap('\xFB01', new char[] { '\x0066',                    /* FB01; 0066 0069; Case map */
                   '\x0069' }),
            new CharMap('\xFB02', new char[] { '\x0066',                    /* FB02; 0066 006C; Case map */
                   '\x006C' }),
            new CharMap('\xFB03', new char[] { '\x0066',               /* FB03; 0066 0066 0069; Case map */
                   '\x0066', '\x0069' }),
            new CharMap('\xFB04', new char[] { '\x0066',               /* FB04; 0066 0066 006C; Case map */
                   '\x0066', '\x006C' }),
            new CharMap('\xFB05', new char[] { '\x0073',                    /* FB05; 0073 0074; Case map */
                   '\x0074' }),
            new CharMap('\xFB06', new char[] { '\x0073',                    /* FB06; 0073 0074; Case map */
                   '\x0074' }),
            new CharMap('\xFB13', new char[] { '\x0574',                    /* FB13; 0574 0576; Case map */
                   '\x0576' }),
            new CharMap('\xFB14', new char[] { '\x0574',                    /* FB14; 0574 0565; Case map */
                   '\x0565' }),
            new CharMap('\xFB15', new char[] { '\x0574',                    /* FB15; 0574 056B; Case map */
                   '\x056B' }),
            new CharMap('\xFB16', new char[] { '\x057E',                    /* FB16; 057E 0576; Case map */
                   '\x0576' }),
            new CharMap('\xFB17', new char[] { '\x0574',                    /* FB17; 0574 056D; Case map */
                   '\x056D' }),
            new CharMap('\xFF21', new char[] { '\xFF41' }),                      /* FF21; FF41; Case map */
            new CharMap('\xFF22', new char[] { '\xFF42' }),                      /* FF22; FF42; Case map */
            new CharMap('\xFF23', new char[] { '\xFF43' }),                      /* FF23; FF43; Case map */
            new CharMap('\xFF24', new char[] { '\xFF44' }),                      /* FF24; FF44; Case map */
            new CharMap('\xFF25', new char[] { '\xFF45' }),                      /* FF25; FF45; Case map */
            new CharMap('\xFF26', new char[] { '\xFF46' }),                      /* FF26; FF46; Case map */
            new CharMap('\xFF27', new char[] { '\xFF47' }),                      /* FF27; FF47; Case map */
            new CharMap('\xFF28', new char[] { '\xFF48' }),                      /* FF28; FF48; Case map */
            new CharMap('\xFF29', new char[] { '\xFF49' }),                      /* FF29; FF49; Case map */
            new CharMap('\xFF2A', new char[] { '\xFF4A' }),                      /* FF2A; FF4A; Case map */
            new CharMap('\xFF2B', new char[] { '\xFF4B' }),                      /* FF2B; FF4B; Case map */
            new CharMap('\xFF2C', new char[] { '\xFF4C' }),                      /* FF2C; FF4C; Case map */
            new CharMap('\xFF2D', new char[] { '\xFF4D' }),                      /* FF2D; FF4D; Case map */
            new CharMap('\xFF2E', new char[] { '\xFF4E' }),                      /* FF2E; FF4E; Case map */
            new CharMap('\xFF2F', new char[] { '\xFF4F' }),                      /* FF2F; FF4F; Case map */
            new CharMap('\xFF30', new char[] { '\xFF50' }),                      /* FF30; FF50; Case map */
            new CharMap('\xFF31', new char[] { '\xFF51' }),                      /* FF31; FF51; Case map */
            new CharMap('\xFF32', new char[] { '\xFF52' }),                      /* FF32; FF52; Case map */
            new CharMap('\xFF33', new char[] { '\xFF53' }),                      /* FF33; FF53; Case map */
            new CharMap('\xFF34', new char[] { '\xFF54' }),                      /* FF34; FF54; Case map */
            new CharMap('\xFF35', new char[] { '\xFF55' }),                      /* FF35; FF55; Case map */
            new CharMap('\xFF36', new char[] { '\xFF56' }),                      /* FF36; FF56; Case map */
            new CharMap('\xFF37', new char[] { '\xFF57' }),                      /* FF37; FF57; Case map */
            new CharMap('\xFF38', new char[] { '\xFF58' }),                      /* FF38; FF58; Case map */
            new CharMap('\xFF39', new char[] { '\xFF59' }),                      /* FF39; FF59; Case map */
            new CharMap('\xFF3A', new char[] { '\xFF5A' }),                      /* FF3A; FF5A; Case map */
        };


        /*
         * C.1.1 ASCII space characters
         * 
         */
        public static readonly Prohibit[] C_1_1 = new Prohibit[]
        {
            new Prohibit('\x0020'),                                     /* 0020; SPACE */
        };


        /*
         * C.1.2 Non-ASCII space characters
         *          */
        public static readonly Prohibit[] C_1_2 = new Prohibit[]
        {
            new Prohibit('\x00A0'),                            /* 00A0; NO-BREAK SPACE */
            new Prohibit('\x1680'),                          /* 1680; OGHAM SPACE MARK */
            new Prohibit('\x2000'),                                   /* 2000; EN QUAD */
            new Prohibit('\x2001'),                                   /* 2001; EM QUAD */
            new Prohibit('\x2002'),                                  /* 2002; EN SPACE */
            new Prohibit('\x2003'),                                  /* 2003; EM SPACE */
            new Prohibit('\x2004'),                        /* 2004; THREE-PER-EM SPACE */
            new Prohibit('\x2005'),                         /* 2005; FOUR-PER-EM SPACE */
            new Prohibit('\x2006'),                          /* 2006; SIX-PER-EM SPACE */
            new Prohibit('\x2007'),                              /* 2007; FIGURE SPACE */
            new Prohibit('\x2008'),                         /* 2008; PUNCTUATION SPACE */
            new Prohibit('\x2009'),                                /* 2009; THIN SPACE */
            new Prohibit('\x200A'),                                /* 200A; HAIR SPACE */
            new Prohibit('\x200B'),                          /* 200B; ZERO WIDTH SPACE */
            new Prohibit('\x202F'),                     /* 202F; NARROW NO-BREAK SPACE */
            new Prohibit('\x205F'),                 /* 205F; MEDIUM MATHEMATICAL SPACE */
            new Prohibit('\x3000'),                         /* 3000; IDEOGRAPHIC SPACE */
        };


        /*
         * C.2.1 ASCII control characters
         * 
         */
        public static readonly Prohibit[] C_2_1 = new Prohibit[]
        {
            new Prohibit('\x0000', '\x001F'),                 /* 0000-001F; [CONTROL CHARACTERS] */
            new Prohibit('\x007F'),                                    /* 007F; DELETE */
        };


        /*
         * C.2.2 Non-ASCII control characters
         * 
         */
        public static readonly Prohibit[] C_2_2 = new Prohibit[]
        {
            new Prohibit('\x0080', '\x009F'),                 /* 0080-009F; [CONTROL CHARACTERS] */
            new Prohibit('\x06DD'),                        /* 06DD; ARABIC END OF AYAH */
            new Prohibit('\x070F'),                  /* 070F; SYRIAC ABBREVIATION MARK */
            new Prohibit('\x180E'),                 /* 180E; MONGOLIAN VOWEL SEPARATOR */
            new Prohibit('\x200C'),                     /* 200C; ZERO WIDTH NON-JOINER */
            new Prohibit('\x200D'),                         /* 200D; ZERO WIDTH JOINER */
            new Prohibit('\x2028'),                            /* 2028; LINE SEPARATOR */
            new Prohibit('\x2029'),                       /* 2029; PARAGRAPH SEPARATOR */
            new Prohibit('\x2060'),                               /* 2060; WORD JOINER */
            new Prohibit('\x2061'),                      /* 2061; FUNCTION APPLICATION */
            new Prohibit('\x2062'),                           /* 2062; INVISIBLE TIMES */
            new Prohibit('\x2063'),                       /* 2063; INVISIBLE SEPARATOR */
            new Prohibit('\x206A', '\x206F'),                 /* 206A-206F; [CONTROL CHARACTERS] */
            new Prohibit('\xFEFF'),                 /* FEFF; ZERO WIDTH NO-BREAK SPACE */
            new Prohibit('\xFFF9', '\xFFFC'),                 /* FFF9-FFFC; [CONTROL CHARACTERS] */
        };


        /*
         * C.3 Private use
         * 
         */
        public static readonly Prohibit[] C_3 = new Prohibit[]
        {
            new Prohibit('\xE000', '\xF8FF'),               /* E000-F8FF; [PRIVATE USE, PLANE 0] */
        };


        /*
         * C.4 Non-character code points
         * 
         */
        public static readonly Prohibit[] C_4 = new Prohibit[]
        {
            new Prohibit('\xFDD0', '\xFDEF'),           /* FDD0-FDEF; [NONCHARACTER CODE POINTS] */
            new Prohibit('\xFFFE', '\xFFFF'),           /* FFFE-FFFF; [NONCHARACTER CODE POINTS] */
        };


        /*
         * C.5 Surrogate codes
         * 
         */
        public static readonly Prohibit[] C_5 = new Prohibit[]
        {
            new Prohibit('\xD800', '\xDFFF'),                    /* D800-DFFF; [SURROGATE CODES] */
        };


        /*
         * C.6 Inappropriate for plain text
         * 
         */
        public static readonly Prohibit[] C_6 = new Prohibit[]
        {
            new Prohibit('\xFFF9'),             /* FFF9; INTERLINEAR ANNOTATION ANCHOR */
            new Prohibit('\xFFFA'),          /* FFFA; INTERLINEAR ANNOTATION SEPARATOR */
            new Prohibit('\xFFFB'),         /* FFFB; INTERLINEAR ANNOTATION TERMINATOR */
            new Prohibit('\xFFFC'),              /* FFFC; OBJECT REPLACEMENT CHARACTER */
            new Prohibit('\xFFFD'),                     /* FFFD; REPLACEMENT CHARACTER */
        };


        /*
         * C.7 Inappropriate for canonical representation
         * 
         */
        public static readonly Prohibit[] C_7 = new Prohibit[]
        {
            new Prohibit('\x2FF0', '\x2FFB'), /* 2FF0-2FFB; [IDEOGRAPHIC DESCRIPTION CHARACTERS] */
        };


        /*
         * C.8 Change display properties or are deprecated
         * 
         */
        public static readonly Prohibit[] C_8 = new Prohibit[]
        {
            new Prohibit('\x0340'),                 /* 0340; COMBINING GRAVE TONE MARK */
            new Prohibit('\x0341'),                 /* 0341; COMBINING ACUTE TONE MARK */
            new Prohibit('\x200E'),                        /* 200E; LEFT-TO-RIGHT MARK */
            new Prohibit('\x200F'),                        /* 200F; RIGHT-TO-LEFT MARK */
            new Prohibit('\x202A'),                   /* 202A; LEFT-TO-RIGHT EMBEDDING */
            new Prohibit('\x202B'),                   /* 202B; RIGHT-TO-LEFT EMBEDDING */
            new Prohibit('\x202C'),                /* 202C; POP DIRECTIONAL FORMATTING */
            new Prohibit('\x202D'),                    /* 202D; LEFT-TO-RIGHT OVERRIDE */
            new Prohibit('\x202E'),                    /* 202E; RIGHT-TO-LEFT OVERRIDE */
            new Prohibit('\x206A'),                /* 206A; INHIBIT SYMMETRIC SWAPPING */
            new Prohibit('\x206B'),               /* 206B; ACTIVATE SYMMETRIC SWAPPING */
            new Prohibit('\x206C'),               /* 206C; INHIBIT ARABIC FORM SHAPING */
            new Prohibit('\x206D'),              /* 206D; ACTIVATE ARABIC FORM SHAPING */
            new Prohibit('\x206E'),                     /* 206E; NATIONAL DIGIT SHAPES */
            new Prohibit('\x206F'),                      /* 206F; NOMINAL DIGIT SHAPES */
        };


        /*
         * C.9 Tagging characters
         * 
         */
        public static readonly Prohibit[] C_9 = new Prohibit[]
        {
        };


        /*
         * D.1 Characters with bidirectional property "R" or "AL"
         * 
         */
        public static readonly Prohibit[] D_1 = new Prohibit[]
        {
            new Prohibit('\x05BE'),                                            /* 05BE */
            new Prohibit('\x05C0'),                                            /* 05C0 */
            new Prohibit('\x05C3'),                                            /* 05C3 */
            new Prohibit('\x05D0', '\x05EA'),                                       /* 05D0-05EA */
            new Prohibit('\x05F0', '\x05F4'),                                       /* 05F0-05F4 */
            new Prohibit('\x061B'),                                            /* 061B */
            new Prohibit('\x061F'),                                            /* 061F */
            new Prohibit('\x0621', '\x063A'),                                       /* 0621-063A */
            new Prohibit('\x0640', '\x064A'),                                       /* 0640-064A */
            new Prohibit('\x066D', '\x066F'),                                       /* 066D-066F */
            new Prohibit('\x0671', '\x06D5'),                                       /* 0671-06D5 */
            new Prohibit('\x06DD'),                                            /* 06DD */
            new Prohibit('\x06E5', '\x06E6'),                                       /* 06E5-06E6 */
            new Prohibit('\x06FA', '\x06FE'),                                       /* 06FA-06FE */
            new Prohibit('\x0700', '\x070D'),                                       /* 0700-070D */
            new Prohibit('\x0710'),                                            /* 0710 */
            new Prohibit('\x0712', '\x072C'),                                       /* 0712-072C */
            new Prohibit('\x0780', '\x07A5'),                                       /* 0780-07A5 */
            new Prohibit('\x07B1'),                                            /* 07B1 */
            new Prohibit('\x200F'),                                            /* 200F */
            new Prohibit('\xFB1D'),                                            /* FB1D */
            new Prohibit('\xFB1F', '\xFB28'),                                       /* FB1F-FB28 */
            new Prohibit('\xFB2A', '\xFB36'),                                       /* FB2A-FB36 */
            new Prohibit('\xFB38', '\xFB3C'),                                       /* FB38-FB3C */
            new Prohibit('\xFB3E'),                                            /* FB3E */
            new Prohibit('\xFB40', '\xFB41'),                                       /* FB40-FB41 */
            new Prohibit('\xFB43', '\xFB44'),                                       /* FB43-FB44 */
            new Prohibit('\xFB46', '\xFBB1'),                                       /* FB46-FBB1 */
            new Prohibit('\xFBD3', '\xFD3D'),                                       /* FBD3-FD3D */
            new Prohibit('\xFD50', '\xFD8F'),                                       /* FD50-FD8F */
            new Prohibit('\xFD92', '\xFDC7'),                                       /* FD92-FDC7 */
            new Prohibit('\xFDF0', '\xFDFC'),                                       /* FDF0-FDFC */
            new Prohibit('\xFE70', '\xFE74'),                                       /* FE70-FE74 */
            new Prohibit('\xFE76', '\xFEFC'),                                       /* FE76-FEFC */
        };


        /*
         * D.2 Characters with bidirectional property "L"
         * 
         */
        public static readonly Prohibit[] D_2 = new Prohibit[]
        {
            new Prohibit('\x0041', '\x005A'),                                       /* 0041-005A */
            new Prohibit('\x0061', '\x007A'),                                       /* 0061-007A */
            new Prohibit('\x00AA'),                                            /* 00AA */
            new Prohibit('\x00B5'),                                            /* 00B5 */
            new Prohibit('\x00BA'),                                            /* 00BA */
            new Prohibit('\x00C0', '\x00D6'),                                       /* 00C0-00D6 */
            new Prohibit('\x00D8', '\x00F6'),                                       /* 00D8-00F6 */
            new Prohibit('\x00F8', '\x0220'),                                       /* 00F8-0220 */
            new Prohibit('\x0222', '\x0233'),                                       /* 0222-0233 */
            new Prohibit('\x0250', '\x02AD'),                                       /* 0250-02AD */
            new Prohibit('\x02B0', '\x02B8'),                                       /* 02B0-02B8 */
            new Prohibit('\x02BB', '\x02C1'),                                       /* 02BB-02C1 */
            new Prohibit('\x02D0', '\x02D1'),                                       /* 02D0-02D1 */
            new Prohibit('\x02E0', '\x02E4'),                                       /* 02E0-02E4 */
            new Prohibit('\x02EE'),                                            /* 02EE */
            new Prohibit('\x037A'),                                            /* 037A */
            new Prohibit('\x0386'),                                            /* 0386 */
            new Prohibit('\x0388', '\x038A'),                                       /* 0388-038A */
            new Prohibit('\x038C'),                                            /* 038C */
            new Prohibit('\x038E', '\x03A1'),                                       /* 038E-03A1 */
            new Prohibit('\x03A3', '\x03CE'),                                       /* 03A3-03CE */
            new Prohibit('\x03D0', '\x03F5'),                                       /* 03D0-03F5 */
            new Prohibit('\x0400', '\x0482'),                                       /* 0400-0482 */
            new Prohibit('\x048A', '\x04CE'),                                       /* 048A-04CE */
            new Prohibit('\x04D0', '\x04F5'),                                       /* 04D0-04F5 */
            new Prohibit('\x04F8', '\x04F9'),                                       /* 04F8-04F9 */
            new Prohibit('\x0500', '\x050F'),                                       /* 0500-050F */
            new Prohibit('\x0531', '\x0556'),                                       /* 0531-0556 */
            new Prohibit('\x0559', '\x055F'),                                       /* 0559-055F */
            new Prohibit('\x0561', '\x0587'),                                       /* 0561-0587 */
            new Prohibit('\x0589'),                                            /* 0589 */
            new Prohibit('\x0903'),                                            /* 0903 */
            new Prohibit('\x0905', '\x0939'),                                       /* 0905-0939 */
            new Prohibit('\x093D', '\x0940'),                                       /* 093D-0940 */
            new Prohibit('\x0949', '\x094C'),                                       /* 0949-094C */
            new Prohibit('\x0950'),                                            /* 0950 */
            new Prohibit('\x0958', '\x0961'),                                       /* 0958-0961 */
            new Prohibit('\x0964', '\x0970'),                                       /* 0964-0970 */
            new Prohibit('\x0982', '\x0983'),                                       /* 0982-0983 */
            new Prohibit('\x0985', '\x098C'),                                       /* 0985-098C */
            new Prohibit('\x098F', '\x0990'),                                       /* 098F-0990 */
            new Prohibit('\x0993', '\x09A8'),                                       /* 0993-09A8 */
            new Prohibit('\x09AA', '\x09B0'),                                       /* 09AA-09B0 */
            new Prohibit('\x09B2'),                                            /* 09B2 */
            new Prohibit('\x09B6', '\x09B9'),                                       /* 09B6-09B9 */
            new Prohibit('\x09BE', '\x09C0'),                                       /* 09BE-09C0 */
            new Prohibit('\x09C7', '\x09C8'),                                       /* 09C7-09C8 */
            new Prohibit('\x09CB', '\x09CC'),                                       /* 09CB-09CC */
            new Prohibit('\x09D7'),                                            /* 09D7 */
            new Prohibit('\x09DC', '\x09DD'),                                       /* 09DC-09DD */
            new Prohibit('\x09DF', '\x09E1'),                                       /* 09DF-09E1 */
            new Prohibit('\x09E6', '\x09F1'),                                       /* 09E6-09F1 */
            new Prohibit('\x09F4', '\x09FA'),                                       /* 09F4-09FA */
            new Prohibit('\x0A05', '\x0A0A'),                                       /* 0A05-0A0A */
            new Prohibit('\x0A0F', '\x0A10'),                                       /* 0A0F-0A10 */
            new Prohibit('\x0A13', '\x0A28'),                                       /* 0A13-0A28 */
            new Prohibit('\x0A2A', '\x0A30'),                                       /* 0A2A-0A30 */
            new Prohibit('\x0A32', '\x0A33'),                                       /* 0A32-0A33 */
            new Prohibit('\x0A35', '\x0A36'),                                       /* 0A35-0A36 */
            new Prohibit('\x0A38', '\x0A39'),                                       /* 0A38-0A39 */
            new Prohibit('\x0A3E', '\x0A40'),                                       /* 0A3E-0A40 */
            new Prohibit('\x0A59', '\x0A5C'),                                       /* 0A59-0A5C */
            new Prohibit('\x0A5E'),                                            /* 0A5E */
            new Prohibit('\x0A66', '\x0A6F'),                                       /* 0A66-0A6F */
            new Prohibit('\x0A72', '\x0A74'),                                       /* 0A72-0A74 */
            new Prohibit('\x0A83'),                                            /* 0A83 */
            new Prohibit('\x0A85', '\x0A8B'),                                       /* 0A85-0A8B */
            new Prohibit('\x0A8D'),                                            /* 0A8D */
            new Prohibit('\x0A8F', '\x0A91'),                                       /* 0A8F-0A91 */
            new Prohibit('\x0A93', '\x0AA8'),                                       /* 0A93-0AA8 */
            new Prohibit('\x0AAA', '\x0AB0'),                                       /* 0AAA-0AB0 */
            new Prohibit('\x0AB2', '\x0AB3'),                                       /* 0AB2-0AB3 */
            new Prohibit('\x0AB5', '\x0AB9'),                                       /* 0AB5-0AB9 */
            new Prohibit('\x0ABD', '\x0AC0'),                                       /* 0ABD-0AC0 */
            new Prohibit('\x0AC9'),                                            /* 0AC9 */
            new Prohibit('\x0ACB', '\x0ACC'),                                       /* 0ACB-0ACC */
            new Prohibit('\x0AD0'),                                            /* 0AD0 */
            new Prohibit('\x0AE0'),                                            /* 0AE0 */
            new Prohibit('\x0AE6', '\x0AEF'),                                       /* 0AE6-0AEF */
            new Prohibit('\x0B02', '\x0B03'),                                       /* 0B02-0B03 */
            new Prohibit('\x0B05', '\x0B0C'),                                       /* 0B05-0B0C */
            new Prohibit('\x0B0F', '\x0B10'),                                       /* 0B0F-0B10 */
            new Prohibit('\x0B13', '\x0B28'),                                       /* 0B13-0B28 */
            new Prohibit('\x0B2A', '\x0B30'),                                       /* 0B2A-0B30 */
            new Prohibit('\x0B32', '\x0B33'),                                       /* 0B32-0B33 */
            new Prohibit('\x0B36', '\x0B39'),                                       /* 0B36-0B39 */
            new Prohibit('\x0B3D', '\x0B3E'),                                       /* 0B3D-0B3E */
            new Prohibit('\x0B40'),                                            /* 0B40 */
            new Prohibit('\x0B47', '\x0B48'),                                       /* 0B47-0B48 */
            new Prohibit('\x0B4B', '\x0B4C'),                                       /* 0B4B-0B4C */
            new Prohibit('\x0B57'),                                            /* 0B57 */
            new Prohibit('\x0B5C', '\x0B5D'),                                       /* 0B5C-0B5D */
            new Prohibit('\x0B5F', '\x0B61'),                                       /* 0B5F-0B61 */
            new Prohibit('\x0B66', '\x0B70'),                                       /* 0B66-0B70 */
            new Prohibit('\x0B83'),                                            /* 0B83 */
            new Prohibit('\x0B85', '\x0B8A'),                                       /* 0B85-0B8A */
            new Prohibit('\x0B8E', '\x0B90'),                                       /* 0B8E-0B90 */
            new Prohibit('\x0B92', '\x0B95'),                                       /* 0B92-0B95 */
            new Prohibit('\x0B99', '\x0B9A'),                                       /* 0B99-0B9A */
            new Prohibit('\x0B9C'),                                            /* 0B9C */
            new Prohibit('\x0B9E', '\x0B9F'),                                       /* 0B9E-0B9F */
            new Prohibit('\x0BA3', '\x0BA4'),                                       /* 0BA3-0BA4 */
            new Prohibit('\x0BA8', '\x0BAA'),                                       /* 0BA8-0BAA */
            new Prohibit('\x0BAE', '\x0BB5'),                                       /* 0BAE-0BB5 */
            new Prohibit('\x0BB7', '\x0BB9'),                                       /* 0BB7-0BB9 */
            new Prohibit('\x0BBE', '\x0BBF'),                                       /* 0BBE-0BBF */
            new Prohibit('\x0BC1', '\x0BC2'),                                       /* 0BC1-0BC2 */
            new Prohibit('\x0BC6', '\x0BC8'),                                       /* 0BC6-0BC8 */
            new Prohibit('\x0BCA', '\x0BCC'),                                       /* 0BCA-0BCC */
            new Prohibit('\x0BD7'),                                            /* 0BD7 */
            new Prohibit('\x0BE7', '\x0BF2'),                                       /* 0BE7-0BF2 */
            new Prohibit('\x0C01', '\x0C03'),                                       /* 0C01-0C03 */
            new Prohibit('\x0C05', '\x0C0C'),                                       /* 0C05-0C0C */
            new Prohibit('\x0C0E', '\x0C10'),                                       /* 0C0E-0C10 */
            new Prohibit('\x0C12', '\x0C28'),                                       /* 0C12-0C28 */
            new Prohibit('\x0C2A', '\x0C33'),                                       /* 0C2A-0C33 */
            new Prohibit('\x0C35', '\x0C39'),                                       /* 0C35-0C39 */
            new Prohibit('\x0C41', '\x0C44'),                                       /* 0C41-0C44 */
            new Prohibit('\x0C60', '\x0C61'),                                       /* 0C60-0C61 */
            new Prohibit('\x0C66', '\x0C6F'),                                       /* 0C66-0C6F */
            new Prohibit('\x0C82', '\x0C83'),                                       /* 0C82-0C83 */
            new Prohibit('\x0C85', '\x0C8C'),                                       /* 0C85-0C8C */
            new Prohibit('\x0C8E', '\x0C90'),                                       /* 0C8E-0C90 */
            new Prohibit('\x0C92', '\x0CA8'),                                       /* 0C92-0CA8 */
            new Prohibit('\x0CAA', '\x0CB3'),                                       /* 0CAA-0CB3 */
            new Prohibit('\x0CB5', '\x0CB9'),                                       /* 0CB5-0CB9 */
            new Prohibit('\x0CBE'),                                            /* 0CBE */
            new Prohibit('\x0CC0', '\x0CC4'),                                       /* 0CC0-0CC4 */
            new Prohibit('\x0CC7', '\x0CC8'),                                       /* 0CC7-0CC8 */
            new Prohibit('\x0CCA', '\x0CCB'),                                       /* 0CCA-0CCB */
            new Prohibit('\x0CD5', '\x0CD6'),                                       /* 0CD5-0CD6 */
            new Prohibit('\x0CDE'),                                            /* 0CDE */
            new Prohibit('\x0CE0', '\x0CE1'),                                       /* 0CE0-0CE1 */
            new Prohibit('\x0CE6', '\x0CEF'),                                       /* 0CE6-0CEF */
            new Prohibit('\x0D02', '\x0D03'),                                       /* 0D02-0D03 */
            new Prohibit('\x0D05', '\x0D0C'),                                       /* 0D05-0D0C */
            new Prohibit('\x0D0E', '\x0D10'),                                       /* 0D0E-0D10 */
            new Prohibit('\x0D12', '\x0D28'),                                       /* 0D12-0D28 */
            new Prohibit('\x0D2A', '\x0D39'),                                       /* 0D2A-0D39 */
            new Prohibit('\x0D3E', '\x0D40'),                                       /* 0D3E-0D40 */
            new Prohibit('\x0D46', '\x0D48'),                                       /* 0D46-0D48 */
            new Prohibit('\x0D4A', '\x0D4C'),                                       /* 0D4A-0D4C */
            new Prohibit('\x0D57'),                                            /* 0D57 */
            new Prohibit('\x0D60', '\x0D61'),                                       /* 0D60-0D61 */
            new Prohibit('\x0D66', '\x0D6F'),                                       /* 0D66-0D6F */
            new Prohibit('\x0D82', '\x0D83'),                                       /* 0D82-0D83 */
            new Prohibit('\x0D85', '\x0D96'),                                       /* 0D85-0D96 */
            new Prohibit('\x0D9A', '\x0DB1'),                                       /* 0D9A-0DB1 */
            new Prohibit('\x0DB3', '\x0DBB'),                                       /* 0DB3-0DBB */
            new Prohibit('\x0DBD'),                                            /* 0DBD */
            new Prohibit('\x0DC0', '\x0DC6'),                                       /* 0DC0-0DC6 */
            new Prohibit('\x0DCF', '\x0DD1'),                                       /* 0DCF-0DD1 */
            new Prohibit('\x0DD8', '\x0DDF'),                                       /* 0DD8-0DDF */
            new Prohibit('\x0DF2', '\x0DF4'),                                       /* 0DF2-0DF4 */
            new Prohibit('\x0E01', '\x0E30'),                                       /* 0E01-0E30 */
            new Prohibit('\x0E32', '\x0E33'),                                       /* 0E32-0E33 */
            new Prohibit('\x0E40', '\x0E46'),                                       /* 0E40-0E46 */
            new Prohibit('\x0E4F', '\x0E5B'),                                       /* 0E4F-0E5B */
            new Prohibit('\x0E81', '\x0E82'),                                       /* 0E81-0E82 */
            new Prohibit('\x0E84'),                                            /* 0E84 */
            new Prohibit('\x0E87', '\x0E88'),                                       /* 0E87-0E88 */
            new Prohibit('\x0E8A'),                                            /* 0E8A */
            new Prohibit('\x0E8D'),                                            /* 0E8D */
            new Prohibit('\x0E94', '\x0E97'),                                       /* 0E94-0E97 */
            new Prohibit('\x0E99', '\x0E9F'),                                       /* 0E99-0E9F */
            new Prohibit('\x0EA1', '\x0EA3'),                                       /* 0EA1-0EA3 */
            new Prohibit('\x0EA5'),                                            /* 0EA5 */
            new Prohibit('\x0EA7'),                                            /* 0EA7 */
            new Prohibit('\x0EAA', '\x0EAB'),                                       /* 0EAA-0EAB */
            new Prohibit('\x0EAD', '\x0EB0'),                                       /* 0EAD-0EB0 */
            new Prohibit('\x0EB2', '\x0EB3'),                                       /* 0EB2-0EB3 */
            new Prohibit('\x0EBD'),                                            /* 0EBD */
            new Prohibit('\x0EC0', '\x0EC4'),                                       /* 0EC0-0EC4 */
            new Prohibit('\x0EC6'),                                            /* 0EC6 */
            new Prohibit('\x0ED0', '\x0ED9'),                                       /* 0ED0-0ED9 */
            new Prohibit('\x0EDC', '\x0EDD'),                                       /* 0EDC-0EDD */
            new Prohibit('\x0F00', '\x0F17'),                                       /* 0F00-0F17 */
            new Prohibit('\x0F1A', '\x0F34'),                                       /* 0F1A-0F34 */
            new Prohibit('\x0F36'),                                            /* 0F36 */
            new Prohibit('\x0F38'),                                            /* 0F38 */
            new Prohibit('\x0F3E', '\x0F47'),                                       /* 0F3E-0F47 */
            new Prohibit('\x0F49', '\x0F6A'),                                       /* 0F49-0F6A */
            new Prohibit('\x0F7F'),                                            /* 0F7F */
            new Prohibit('\x0F85'),                                            /* 0F85 */
            new Prohibit('\x0F88', '\x0F8B'),                                       /* 0F88-0F8B */
            new Prohibit('\x0FBE', '\x0FC5'),                                       /* 0FBE-0FC5 */
            new Prohibit('\x0FC7', '\x0FCC'),                                       /* 0FC7-0FCC */
            new Prohibit('\x0FCF'),                                            /* 0FCF */
            new Prohibit('\x1000', '\x1021'),                                       /* 1000-1021 */
            new Prohibit('\x1023', '\x1027'),                                       /* 1023-1027 */
            new Prohibit('\x1029', '\x102A'),                                       /* 1029-102A */
            new Prohibit('\x102C'),                                            /* 102C */
            new Prohibit('\x1031'),                                            /* 1031 */
            new Prohibit('\x1038'),                                            /* 1038 */
            new Prohibit('\x1040', '\x1057'),                                       /* 1040-1057 */
            new Prohibit('\x10A0', '\x10C5'),                                       /* 10A0-10C5 */
            new Prohibit('\x10D0', '\x10F8'),                                       /* 10D0-10F8 */
            new Prohibit('\x10FB'),                                            /* 10FB */
            new Prohibit('\x1100', '\x1159'),                                       /* 1100-1159 */
            new Prohibit('\x115F', '\x11A2'),                                       /* 115F-11A2 */
            new Prohibit('\x11A8', '\x11F9'),                                       /* 11A8-11F9 */
            new Prohibit('\x1200', '\x1206'),                                       /* 1200-1206 */
            new Prohibit('\x1208', '\x1246'),                                       /* 1208-1246 */
            new Prohibit('\x1248'),                                            /* 1248 */
            new Prohibit('\x124A', '\x124D'),                                       /* 124A-124D */
            new Prohibit('\x1250', '\x1256'),                                       /* 1250-1256 */
            new Prohibit('\x1258'),                                            /* 1258 */
            new Prohibit('\x125A', '\x125D'),                                       /* 125A-125D */
            new Prohibit('\x1260', '\x1286'),                                       /* 1260-1286 */
            new Prohibit('\x1288'),                                            /* 1288 */
            new Prohibit('\x128A', '\x128D'),                                       /* 128A-128D */
            new Prohibit('\x1290', '\x12AE'),                                       /* 1290-12AE */
            new Prohibit('\x12B0'),                                            /* 12B0 */
            new Prohibit('\x12B2', '\x12B5'),                                       /* 12B2-12B5 */
            new Prohibit('\x12B8', '\x12BE'),                                       /* 12B8-12BE */
            new Prohibit('\x12C0'),                                            /* 12C0 */
            new Prohibit('\x12C2', '\x12C5'),                                       /* 12C2-12C5 */
            new Prohibit('\x12C8', '\x12CE'),                                       /* 12C8-12CE */
            new Prohibit('\x12D0', '\x12D6'),                                       /* 12D0-12D6 */
            new Prohibit('\x12D8', '\x12EE'),                                       /* 12D8-12EE */
            new Prohibit('\x12F0', '\x130E'),                                       /* 12F0-130E */
            new Prohibit('\x1310'),                                            /* 1310 */
            new Prohibit('\x1312', '\x1315'),                                       /* 1312-1315 */
            new Prohibit('\x1318', '\x131E'),                                       /* 1318-131E */
            new Prohibit('\x1320', '\x1346'),                                       /* 1320-1346 */
            new Prohibit('\x1348', '\x135A'),                                       /* 1348-135A */
            new Prohibit('\x1361', '\x137C'),                                       /* 1361-137C */
            new Prohibit('\x13A0', '\x13F4'),                                       /* 13A0-13F4 */
            new Prohibit('\x1401', '\x1676'),                                       /* 1401-1676 */
            new Prohibit('\x1681', '\x169A'),                                       /* 1681-169A */
            new Prohibit('\x16A0', '\x16F0'),                                       /* 16A0-16F0 */
            new Prohibit('\x1700', '\x170C'),                                       /* 1700-170C */
            new Prohibit('\x170E', '\x1711'),                                       /* 170E-1711 */
            new Prohibit('\x1720', '\x1731'),                                       /* 1720-1731 */
            new Prohibit('\x1735', '\x1736'),                                       /* 1735-1736 */
            new Prohibit('\x1740', '\x1751'),                                       /* 1740-1751 */
            new Prohibit('\x1760', '\x176C'),                                       /* 1760-176C */
            new Prohibit('\x176E', '\x1770'),                                       /* 176E-1770 */
            new Prohibit('\x1780', '\x17B6'),                                       /* 1780-17B6 */
            new Prohibit('\x17BE', '\x17C5'),                                       /* 17BE-17C5 */
            new Prohibit('\x17C7', '\x17C8'),                                       /* 17C7-17C8 */
            new Prohibit('\x17D4', '\x17DA'),                                       /* 17D4-17DA */
            new Prohibit('\x17DC'),                                            /* 17DC */
            new Prohibit('\x17E0', '\x17E9'),                                       /* 17E0-17E9 */
            new Prohibit('\x1810', '\x1819'),                                       /* 1810-1819 */
            new Prohibit('\x1820', '\x1877'),                                       /* 1820-1877 */
            new Prohibit('\x1880', '\x18A8'),                                       /* 1880-18A8 */
            new Prohibit('\x1E00', '\x1E9B'),                                       /* 1E00-1E9B */
            new Prohibit('\x1EA0', '\x1EF9'),                                       /* 1EA0-1EF9 */
            new Prohibit('\x1F00', '\x1F15'),                                       /* 1F00-1F15 */
            new Prohibit('\x1F18', '\x1F1D'),                                       /* 1F18-1F1D */
            new Prohibit('\x1F20', '\x1F45'),                                       /* 1F20-1F45 */
            new Prohibit('\x1F48', '\x1F4D'),                                       /* 1F48-1F4D */
            new Prohibit('\x1F50', '\x1F57'),                                       /* 1F50-1F57 */
            new Prohibit('\x1F59'),                                            /* 1F59 */
            new Prohibit('\x1F5B'),                                            /* 1F5B */
            new Prohibit('\x1F5D'),                                            /* 1F5D */
            new Prohibit('\x1F5F', '\x1F7D'),                                       /* 1F5F-1F7D */
            new Prohibit('\x1F80', '\x1FB4'),                                       /* 1F80-1FB4 */
            new Prohibit('\x1FB6', '\x1FBC'),                                       /* 1FB6-1FBC */
            new Prohibit('\x1FBE'),                                            /* 1FBE */
            new Prohibit('\x1FC2', '\x1FC4'),                                       /* 1FC2-1FC4 */
            new Prohibit('\x1FC6', '\x1FCC'),                                       /* 1FC6-1FCC */
            new Prohibit('\x1FD0', '\x1FD3'),                                       /* 1FD0-1FD3 */
            new Prohibit('\x1FD6', '\x1FDB'),                                       /* 1FD6-1FDB */
            new Prohibit('\x1FE0', '\x1FEC'),                                       /* 1FE0-1FEC */
            new Prohibit('\x1FF2', '\x1FF4'),                                       /* 1FF2-1FF4 */
            new Prohibit('\x1FF6', '\x1FFC'),                                       /* 1FF6-1FFC */
            new Prohibit('\x200E'),                                            /* 200E */
            new Prohibit('\x2071'),                                            /* 2071 */
            new Prohibit('\x207F'),                                            /* 207F */
            new Prohibit('\x2102'),                                            /* 2102 */
            new Prohibit('\x2107'),                                            /* 2107 */
            new Prohibit('\x210A', '\x2113'),                                       /* 210A-2113 */
            new Prohibit('\x2115'),                                            /* 2115 */
            new Prohibit('\x2119', '\x211D'),                                       /* 2119-211D */
            new Prohibit('\x2124'),                                            /* 2124 */
            new Prohibit('\x2126'),                                            /* 2126 */
            new Prohibit('\x2128'),                                            /* 2128 */
            new Prohibit('\x212A', '\x212D'),                                       /* 212A-212D */
            new Prohibit('\x212F', '\x2131'),                                       /* 212F-2131 */
            new Prohibit('\x2133', '\x2139'),                                       /* 2133-2139 */
            new Prohibit('\x213D', '\x213F'),                                       /* 213D-213F */
            new Prohibit('\x2145', '\x2149'),                                       /* 2145-2149 */
            new Prohibit('\x2160', '\x2183'),                                       /* 2160-2183 */
            new Prohibit('\x2336', '\x237A'),                                       /* 2336-237A */
            new Prohibit('\x2395'),                                            /* 2395 */
            new Prohibit('\x249C', '\x24E9'),                                       /* 249C-24E9 */
            new Prohibit('\x3005', '\x3007'),                                       /* 3005-3007 */
            new Prohibit('\x3021', '\x3029'),                                       /* 3021-3029 */
            new Prohibit('\x3031', '\x3035'),                                       /* 3031-3035 */
            new Prohibit('\x3038', '\x303C'),                                       /* 3038-303C */
            new Prohibit('\x3041', '\x3096'),                                       /* 3041-3096 */
            new Prohibit('\x309D', '\x309F'),                                       /* 309D-309F */
            new Prohibit('\x30A1', '\x30FA'),                                       /* 30A1-30FA */
            new Prohibit('\x30FC', '\x30FF'),                                       /* 30FC-30FF */
            new Prohibit('\x3105', '\x312C'),                                       /* 3105-312C */
            new Prohibit('\x3131', '\x318E'),                                       /* 3131-318E */
            new Prohibit('\x3190', '\x31B7'),                                       /* 3190-31B7 */
            new Prohibit('\x31F0', '\x321C'),                                       /* 31F0-321C */
            new Prohibit('\x3220', '\x3243'),                                       /* 3220-3243 */
            new Prohibit('\x3260', '\x327B'),                                       /* 3260-327B */
            new Prohibit('\x327F', '\x32B0'),                                       /* 327F-32B0 */
            new Prohibit('\x32C0', '\x32CB'),                                       /* 32C0-32CB */
            new Prohibit('\x32D0', '\x32FE'),                                       /* 32D0-32FE */
            new Prohibit('\x3300', '\x3376'),                                       /* 3300-3376 */
            new Prohibit('\x337B', '\x33DD'),                                       /* 337B-33DD */
            new Prohibit('\x33E0', '\x33FE'),                                       /* 33E0-33FE */
            new Prohibit('\x3400', '\x4DB5'),                                       /* 3400-4DB5 */
            new Prohibit('\x4E00', '\x9FA5'),                                       /* 4E00-9FA5 */
            new Prohibit('\xA000', '\xA48C'),                                       /* A000-A48C */
            new Prohibit('\xAC00', '\xD7A3'),                                       /* AC00-D7A3 */
            new Prohibit('\xD800', '\xFA2D'),                                       /* D800-FA2D */
            new Prohibit('\xFA30', '\xFA6A'),                                       /* FA30-FA6A */
            new Prohibit('\xFB00', '\xFB06'),                                       /* FB00-FB06 */
            new Prohibit('\xFB13', '\xFB17'),                                       /* FB13-FB17 */
            new Prohibit('\xFF21', '\xFF3A'),                                       /* FF21-FF3A */
            new Prohibit('\xFF41', '\xFF5A'),                                       /* FF41-FF5A */
            new Prohibit('\xFF66', '\xFFBE'),                                       /* FF66-FFBE */
            new Prohibit('\xFFC2', '\xFFC7'),                                       /* FFC2-FFC7 */
            new Prohibit('\xFFCA', '\xFFCF'),                                       /* FFCA-FFCF */
            new Prohibit('\xFFD2', '\xFFD7'),                                       /* FFD2-FFD7 */
            new Prohibit('\xFFDA', '\xFFDC'),                                       /* FFDA-FFDC */
        };

    }
}
