﻿using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class SurfaceParser
    {
        static public Surface Parse(string filePath)
        {
            string? line;
            string[]? splittedArray;
            string[] elements;
            string row;

            bool isFirst;
            bool isFirstHeightValue = true;

            StreamReader reader = new(filePath, Encoding.UTF8);
            line = reader.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Empty file.");
                return new Surface(0, 0);
            }
            splittedArray = line?.Split(' ');

            int sizeI = Convert.ToInt32(splittedArray[0], CultureInfo.InvariantCulture);
            int sizeJ = Convert.ToInt32(splittedArray[1], CultureInfo.InvariantCulture);
            Surface surface = new (sizeI, sizeJ);

            int vertexNumber;
            int i, j;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                splittedArray = line?.Split(';');
                isFirst = true;
                i = j = vertexNumber = 0;

                foreach (var obj in splittedArray)
                {

                    row = obj[0] == ' ' ? obj.Remove(0, 1) : obj;
                    elements = row.Split(' ');

                    float minHeight, maxHeight;

                    if (isFirst)
                    {
                        i = Convert.ToInt32(elements[0], CultureInfo.InvariantCulture);
                        j = Convert.ToInt32(elements[1], CultureInfo.InvariantCulture);
                        bool isActive = Convert.ToBoolean(Convert.ToInt32(elements[2], CultureInfo.InvariantCulture));
                        surface.GetQuad(i, j) = new()
                        {
                            isActive = isActive
                        };
                        isFirst = false;
                        if (!isActive)
                            surface.Size--;
                        continue;
                    }
                    else
                    {

                        if (isFirstHeightValue)
                        {
                            minHeight = maxHeight = Convert.ToSingle(elements[1], CultureInfo.InvariantCulture);
                            isFirstHeightValue = false;
                        }

                        var coords = new Vector3(
                            Convert.ToSingle(elements[0], CultureInfo.InvariantCulture),
                            Convert.ToSingle(elements[1], CultureInfo.InvariantCulture),
                            Convert.ToSingle(elements[2], CultureInfo.InvariantCulture));
                        surface.GetQuad(i, j).vertices[vertexNumber++] = coords;
                        surface.GetQuad(i, j).property = coords.Y;
                    }
                }
            }
            return surface;
        }
    }
}