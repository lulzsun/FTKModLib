namespace FTKModLib.Objects {
    public class CustomLocalizedString {
        public string _en; public string _fr; public string _it;
        public string _de; public string _es; public string _pt_br;
        public string _ru; public string _zh_cn; public string _zh_tw;
        public string _pl; public string _ja; public string _ko;


        /// <summary>
        /// This class provides multi-language support for a string
        /// </summary>
        /// <param name="__en"></param>
        /// <param name="__fr"></param>
        /// <param name="__it"></param>
        /// <param name="__de"></param>
        /// <param name="__es"></param>
        /// <param name="__pt_br"></param>
        /// <param name="__ru"></param>
        /// <param name="__zh_cn"></param>
        /// <param name="__zh_tw"></param>
        /// <param name="__pl"></param>
        /// <param name="__ja"></param>
        /// <param name="__ko"></param>
        public CustomLocalizedString(string __en = "", string __fr = "", string __it = "", string __de = "", string __es = "", string __pt_br = "", string __ru = "", string __zh_cn = "", string __zh_tw = "", string __pl = "", string __ja = "", string __ko = "") {
            _en = __en; _fr = __fr; _it = __it;
            _de = __de; _es = __es; _pt_br = __pt_br;
            _ru = __ru; _ko = __ko; _zh_cn = __zh_cn;
            _zh_tw = __zh_tw; _pl = __pl; _ja = __ja;
        }

    }
}
