﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码由 wsdl 自动生成, Version=4.6.1055.0。
// 
namespace xcOBDServices
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "xcPDAServiceSoap", Namespace = "http://www.cdxcjc.com/")]
    public partial class xcPDAService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback GetInterfaceVersionOperationCompleted;

        private System.Threading.SendOrPostCallback QueryOperationCompleted;

        /// <remarks/>
        public xcPDAService()
        {
            this.Url = "http://192.168.1.100:9001/xcPDAService.asmx";
        }
        public xcPDAService(string url)
        {
            this.Url = url;
        }

        /// <remarks/>
        public event GetInterfaceVersionCompletedEventHandler GetInterfaceVersionCompleted;

        /// <remarks/>
        public event QueryCompletedEventHandler QueryCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cdxcjc.com/GetInterfaceVersion", RequestNamespace = "http://www.cdxcjc.com/", ResponseNamespace = "http://www.cdxcjc.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetInterfaceVersion()
        {
            object[] results = this.Invoke("GetInterfaceVersion", new object[0]);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetInterfaceVersion(System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetInterfaceVersion", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public string EndGetInterfaceVersion(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void GetInterfaceVersionAsync()
        {
            this.GetInterfaceVersionAsync(null);
        }

        /// <remarks/>
        public void GetInterfaceVersionAsync(object userState)
        {
            if ((this.GetInterfaceVersionOperationCompleted == null))
            {
                this.GetInterfaceVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInterfaceVersionOperationCompleted);
            }
            this.InvokeAsync("GetInterfaceVersion", new object[0], this.GetInterfaceVersionOperationCompleted, userState);
        }

        private void OnGetInterfaceVersionOperationCompleted(object arg)
        {
            if ((this.GetInterfaceVersionCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInterfaceVersionCompleted(this, new GetInterfaceVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.cdxcjc.com/Query", RequestNamespace = "http://www.cdxcjc.com/", ResponseNamespace = "http://www.cdxcjc.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Query(string jkid, string xml)
        {
            object[] results = this.Invoke("Query", new object[] {
                        jkid,
                        xml});
            DataUpdate.FileOpreate.SaveLog("[Send]\r\njkid=\r\n" + jkid + "\r\n" + "xml=\r\n" + xml,"XCPDA_LOG",3);
            DataUpdate.FileOpreate.SaveLog("[Receive]\r\nResult=\r\n" + (string)(results[0]), "XCPDA_LOG", 3);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginQuery(string jkid, string xml, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Query", new object[] {
                        jkid,
                        xml}, callback, asyncState);
        }

        /// <remarks/>
        public string EndQuery(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void QueryAsync(string jkid, string xml)
        {
            this.QueryAsync(jkid, xml, null);
        }

        /// <remarks/>
        public void QueryAsync(string jkid, string xml, object userState)
        {
            if ((this.QueryOperationCompleted == null))
            {
                this.QueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnQueryOperationCompleted);
            }
            this.InvokeAsync("Query", new object[] {
                        jkid,
                        xml}, this.QueryOperationCompleted, userState);
        }

        private void OnQueryOperationCompleted(object arg)
        {
            if ((this.QueryCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.QueryCompleted(this, new QueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void GetInterfaceVersionCompletedEventHandler(object sender, GetInterfaceVersionCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInterfaceVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetInterfaceVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    public delegate void QueryCompletedEventHandler(object sender, QueryCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal QueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}