﻿/*
 * Copyright 2012 ZXing.Net authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

using ZXing.Common;
using ZXing.Rendering;

namespace ZXing
{
   /// <summary>
   /// A base class for specific barcode writers with specific formats of barcode images.
   /// </summary>
   /// <typeparam name="TOutput">The type of the output.</typeparam>
   public class BarcodeWriterGeneric<TOutput> : IBarcodeWriterGeneric<TOutput>
   {
      private EncodingOptions options;

      /// <summary>
      /// Returns or sets the barcode format.
      /// The value is only suitable if the MultiFormatWriter is used.
      /// </summary>
      public BarcodeFormat Format { get; set; }

      /// <summary>
      /// Returns or sets the options container for the encoding and renderer process.
      /// </summary>
      public EncodingOptions Options
      {
         get
         {
            return (options ?? (options = new EncodingOptions { Height = 100, Width = 100 }));
         }
         set
         {
            options = value;
         }
      }

      /// <summary>
      /// Returns or sets the writer which encodes the content to a BitMatrix.
      /// If no value is set the MultiFormatWriter is used.
      /// </summary>
      public Writer Encoder { get; set; }

      /// <summary>
      /// Returns or sets the renderer which should be used to render the encoded BitMatrix.
      /// </summary>
      public IBarcodeRenderer<TOutput> Renderer { get; set; }

      /// <summary>
      /// Encodes the specified contents and returns a BitMatrix array.
      /// That array has to be rendered manually or with a IBarcodeRenderer.
      /// </summary>
      /// <param name="contents">The contents.</param>
      /// <returns></returns>
      public BitMatrix Encode(string contents)
      {
         var encoder = Encoder ?? new MultiFormatWriter();
         var currentOptions = Options;
         return encoder.encode(contents, Format, currentOptions.Width, currentOptions.Height - currentOptions.VerticalOffset, currentOptions.Hints);
      }

      /// <summary>
      /// Encodes the specified contents and returns a rendered instance of the barcode.
      /// For rendering the instance of the property Renderer is used and has to be set before
      /// calling that method.
      /// </summary>
      /// <param name="contents">The contents.</param>
      /// <returns></returns>
      public TOutput Write(string contents)
      {
         if (Renderer == null)
         {
            throw new InvalidOperationException("You have to set a renderer instance.");
         }

         var matrix = Encode(contents);

         return Renderer.Render(matrix, Format, contents, Options);
      }

      /// <summary>
      /// Returns a rendered instance of the barcode which is given by a BitMatrix.
      /// For rendering the instance of the property Renderer is used and has to be set before
      /// calling that method.
      /// </summary>
      /// <param name="matrix">The matrix.</param>
      /// <returns></returns>
      public TOutput Write(BitMatrix matrix)
      {
         if (Renderer == null)
         {
            throw new InvalidOperationException("You have to set a renderer instance.");
         }

         return Renderer.Render(matrix, Format, null, Options);
      }
   }
}
