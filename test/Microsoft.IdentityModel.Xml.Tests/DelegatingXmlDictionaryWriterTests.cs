//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Tests;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class DelegatingXmlDictionaryWriterTests
    {
        [Theory, MemberData(nameof(WriteXmlTheoryData))]
        public void WriteXml(DelegatingXmlDictionaryWriterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteXml", theoryData);
            try
            {
                theoryData.DelegatingWriter.WriteStartElement("name", "prefix", "namespace");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DelegatingXmlDictionaryWriterTheoryData> WriteXmlTheoryData
        {
            get
            {
                return new TheoryData<DelegatingXmlDictionaryWriterTheoryData>
                {
                    new DelegatingXmlDictionaryWriterTheoryData
                    {
                        DelegatingWriter = new DelegatingXmlDictionaryWriterPublic(),
                        ExpectedException = ExpectedException.InvalidOperationException("IDX30028:"),
                        First = true,
                        TestId = "InnerWriter-Null"
                    },
                    new DelegatingXmlDictionaryWriterTheoryData
                    {
                        DelegatingWriter = new DelegatingXmlDictionaryWriterPublic
                        {
                            InnerWriterPublic = XmlDictionaryWriter.CreateBinaryWriter(new MemoryStream())
                        },
                        TestId = "InnerWriter-Set"
                    }
                };
            }
        }
    }

    public class DelegatingXmlDictionaryWriterPublic : DelegatingXmlDictionaryWriter
    {
        public XmlDictionaryWriter InnerWriterPublic
        {
            get => InnerWriter;
            set => InnerWriter = value;
        }
    }

    public class DelegatingXmlDictionaryWriterTheoryData : TheoryDataBase
    {
        public DelegatingXmlDictionaryWriterPublic DelegatingWriter { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
