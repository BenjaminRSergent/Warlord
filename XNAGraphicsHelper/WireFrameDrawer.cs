using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGraphicsHelper
{
    // Watch out, there be dragons here.
    // This is a class full of ugly duplication. Works well, 
    // but it should be expressed without so much copy-pasta.
    public class WireFrameDrawer
    {
        private GraphicsDevice graphics;
        private BasicEffect effect;

        public WireFrameDrawer(GraphicsDevice graphicsDevice)
        {
            graphics = graphicsDevice;
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
        }
        public void DrawBoundingBox(BoundingBox box, Matrix view, Matrix projection)
        {
            VertexPositionColor[] vertices = GetBoundingBoxLineList(box, Color.Red, Color.Blue, Color.Green);

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length/2);
            }
        }
        public void DrawBoundingBoxs(BoundingBox[] box, Matrix view, Matrix projection)
        {
            const int PointsInBox = 24;
            VertexPositionColor[] totalVertices = new VertexPositionColor[box.Length*PointsInBox];

            VertexPositionColor[] boxVerticies;
            for(int outerIndex = 0; outerIndex < box.Length; outerIndex++)
            {
                boxVerticies = GetBoundingBoxLineList(box[outerIndex], Color.Red, Color.Blue, Color.Green);
                
                for(int innerIndex = 0; innerIndex < boxVerticies.Length; innerIndex++)
                {
                    totalVertices[outerIndex*boxVerticies.Length+innerIndex] = boxVerticies[innerIndex];
                }
            }            

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineList, totalVertices, 0, totalVertices.Length/2);
            }
        }
        public void DrawLine(Vector3 start, Vector3 end, Color color, Matrix view, Matrix projection)
        {
            if(effect == null)
                return;

            VertexPositionColor[] vertices = new VertexPositionColor[2];

            vertices[0].Position = start;
            vertices[1].Position = end;
            vertices[0].Color = color;
            vertices[1].Color = color;

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, 1);
            }
        }
        public void DrawLineStrip(Vector3[] points, Color color, Matrix view, Matrix projection)
        {
            if(effect == null || points.Length == 0)
                return;

            VertexPositionColor[] vertices = new VertexPositionColor[points.Length];

            for(int index = 0; index < points.Length; index++)
            {
                vertices[index].Position = points[index];
                vertices[index].Color = color;
            }

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
            }
        }
        public void DrawLineList(Vector3[] points, Color color, Matrix view, Matrix projection)
        {
            if(effect == null || points.Length == 0)
                return;

            VertexPositionColor[] vertices = new VertexPositionColor[points.Length];

            for(int index = 0; index < points.Length; index++)
            {
                vertices[index].Position = points[index];
                vertices[index].Color = color;
            }

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length - 1);
            }
        }
        public void DrawLineStrip(Vector3[] points, Color[] color, Matrix view, Matrix projection)
        {
            if(effect == null || points.Length == 0)
                return;

            VertexPositionColor[] vertices = new VertexPositionColor[points.Length];

            for(int index = 0; index < points.Length; index++)
            {
                vertices[index].Position = points[index];
                vertices[index].Color = color[index];
            }

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
            }
        }
        public void DrawLineList(Vector3[] points, Color[] color, Matrix view, Matrix projection)
        {
            if(effect == null || points.Length == 0)
                return;

            VertexPositionColor[] vertices = new VertexPositionColor[points.Length];

            for(int index = 0; index < points.Length; index++)
            {
                vertices[index].Position = points[index];
                vertices[index].Color = color[index];
            }

            effect.View = view;
            effect.Projection = projection;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length - 1);
            }
        }
        private VertexPositionColor[] GetBoundingBoxLineList(BoundingBox box, Color xColor,Color yColor,Color zColor)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[24];

            Vector3 leftLowerBack = new Vector3(box.Min.X,box.Min.Y,box.Min.Z);
            Vector3 leftLowerFront = new Vector3(box.Min.X,box.Min.Y,box.Max.Z);

            Vector3 leftUpperBack = new Vector3(box.Min.X,box.Max.Y,box.Min.Z);
            Vector3 leftUpperFront = new Vector3(box.Min.X,box.Max.Y,box.Max.Z);

            Vector3 rightLowerBack = new Vector3(box.Max.X,box.Min.Y,box.Min.Z);
            Vector3 rightLowerFront = new Vector3(box.Max.X,box.Min.Y,box.Max.Z);

            Vector3 rightUpperBack = new Vector3(box.Max.X,box.Max.Y,box.Min.Z);
            Vector3 rightUpperFront = new Vector3(box.Max.X,box.Max.Y,box.Max.Z);

            //X-Lines            
            vertices[16].Position = leftLowerBack;
            vertices[17].Position = rightLowerBack;
            vertices[16].Color = xColor;
            vertices[17].Color = xColor;

            vertices[18].Position = leftLowerFront;
            vertices[19].Position = rightLowerFront;
            vertices[18].Color = xColor;
            vertices[19].Color = xColor;

            vertices[20].Position = leftUpperBack;
            vertices[21].Position = rightUpperBack;
            vertices[20].Color = xColor;
            vertices[21].Color = xColor;

            vertices[22].Position = leftUpperFront;
            vertices[23].Position = rightUpperFront;
            vertices[22].Color = xColor;
            vertices[23].Color = xColor;

            //Y-Lines            
            vertices[8].Position = leftLowerBack;
            vertices[9].Position = leftUpperBack;
            vertices[8].Color = yColor;
            vertices[9].Color = yColor;

            vertices[10].Position = leftLowerFront;
            vertices[11].Position = leftUpperFront;
            vertices[10].Color = yColor;
            vertices[11].Color = yColor;

            vertices[12].Position = rightLowerBack;
            vertices[13].Position = rightUpperBack;
            vertices[12].Color = yColor;
            vertices[13].Color = yColor;

            vertices[14].Position = rightLowerFront;
            vertices[15].Position = rightUpperFront;
            vertices[14].Color = yColor;
            vertices[15].Color = yColor;

            //Z-Lines
            vertices[0].Position = leftLowerBack;
            vertices[1].Position = leftLowerFront;
            vertices[0].Color = zColor;
            vertices[1].Color = zColor;

            vertices[2].Position = rightLowerBack;
            vertices[3].Position = rightLowerFront;
            vertices[2].Color = zColor;
            vertices[3].Color = zColor;

            vertices[4].Position = leftUpperBack;
            vertices[5].Position = leftUpperFront;
            vertices[4].Color = zColor;
            vertices[5].Color = zColor;

            vertices[6].Position = rightUpperBack;
            vertices[7].Position = rightUpperFront;
            vertices[6].Color = zColor;
            vertices[7].Color = zColor;

            return vertices;
        }
    }
}
