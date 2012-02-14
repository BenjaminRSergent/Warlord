using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAGraphicsHelper
{
    public class TextDrawer
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;        

        public TextDrawer(GraphicsDevice graphics, SpriteFont spriteFont)
        {
            this.spriteBatch = new SpriteBatch(graphics);
            this.spriteFont = spriteFont;    
            this.graphics = graphics;
        }

        public void DrawText2D(string text, Vector2 position, Color color)
        {
            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;

            spriteBatch.Begin( );
                spriteBatch.DrawString(spriteFont, text, position, color);
            spriteBatch.End( );

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;
        }
        public void DrawText2D(string[] text, Vector2[] position, Color[] color)
        {
            if( text.Length > position.Length || text.Length > color.Length )
                return;

            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;

            spriteBatch.Begin( );
            for( int index = 0; index < text.Length; index++)      
                spriteBatch.DrawString(spriteFont, text[index], position[index], color[index]);
            spriteBatch.End( );

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;
        }
        public void DrawText3D(string text, Vector3 position, Color color, Matrix view, Matrix projection, float scale)
        {
            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;       
     
            float height = spriteFont.MeasureString(text).Y;

            Vector3 transformedPosition = graphics.Viewport.Project(position, projection, view, Matrix.Identity);
            float distance = (position - view.Translation).Length( );

            if(transformedPosition.Z < 1 && distance > 0 && height*scale/distance > 2 )
            { 
                spriteBatch.Begin( );
                    spriteBatch.DrawString(spriteFont,
                                           text,
                                           new Vector2(transformedPosition.X, transformedPosition.Y),                                 
                                           color,
                                           0,
                                           Vector2.Zero,
                                           scale/distance,
                                           SpriteEffects.None,
                                           0);
                spriteBatch.End( );
            }

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;
        }
        public void DrawText3D(string[] text, Vector3[] position, Color[] color, Vector3 cameraPosition, Matrix view, Matrix projection, float[] scale)
        {
            if( position.Length < text.Length || color.Length < text.Length || scale.Length < text.Length )
                return;

            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;

            float[] height  = new float[text.Length];
            Vector3[] transformedPosition = new Vector3[text.Length];
            float[] distance = new float[text.Length];

            for(int index = 0; index < text.Length; index++)
            { 
                height[index] = spriteFont.MeasureString(text[index]).Y;

                transformedPosition[index] = graphics.Viewport.Project(position[index], projection, view, Matrix.Identity);
                distance[index] = (position[index] - cameraPosition).Length( );
            }

            spriteBatch.Begin( );

            for(int index = 0; index < text.Length; index++)
            { 
                
                if(transformedPosition[index].Z < 1 && distance[index] > 0 && height[index]*scale[index]/distance[index] > 2 )
                { 
                    spriteBatch.DrawString(spriteFont,
                                            text[index],
                                            new Vector2(transformedPosition[index].X, transformedPosition[index].Y),                                 
                                            color[index],
                                            0,
                                            Vector2.Zero,
                                            scale[index]/distance[index],
                                            SpriteEffects.None,
                                            0);
                }
            }
            spriteBatch.End( );

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;            
        }
        public void DrawText3D(string[] text, Vector3[] position, Color[] color, Vector3 cameraPosition, Matrix view, Matrix projection, float scale)
        {
            if( position.Length < text.Length || color.Length < text.Length )
                return;

            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;

            float[] height  = new float[text.Length];
            Vector3[] transformedPosition = new Vector3[text.Length];
            float[] distance = new float[text.Length];

            for(int index = 0; index < text.Length; index++)
            { 
                height[index] = spriteFont.MeasureString(text[index]).Y;

                transformedPosition[index] = graphics.Viewport.Project(position[index], projection, view, Matrix.Identity);
                distance[index] = (position[index] - cameraPosition).Length( );
            }

            spriteBatch.Begin( );

            for(int index = 0; index < text.Length; index++)
            { 
                
                if(transformedPosition[index].Z < 1 && distance[index] > 0 && height[index]*scale/distance[index] > 2 )
                { 
                    spriteBatch.DrawString(spriteFont,
                                            text[index],
                                            new Vector2(transformedPosition[index].X, transformedPosition[index].Y),                                 
                                            color[index],
                                            0,
                                            Vector2.Zero,
                                            scale/distance[index],
                                            SpriteEffects.None,
                                            0);
                }
            }
            spriteBatch.End( );

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;            
        }
        public void DrawText3D(string[] text, Vector3[] position, Color color, Vector3 cameraPosition, Matrix view, Matrix projection, float scale)
        {
            if( position.Length < text.Length )
                return;

            BlendState bs = graphics.BlendState;
            DepthStencilState ds = graphics.DepthStencilState;

            float[] height  = new float[text.Length];
            Vector3[] transformedPosition = new Vector3[text.Length];
            float[] distance = new float[text.Length];

            for(int index = 0; index < text.Length; index++)
            { 
                height[index] = spriteFont.MeasureString(text[index]).Y;

                transformedPosition[index] = graphics.Viewport.Project(position[index], projection, view, Matrix.Identity);
                distance[index] = (position[index] - cameraPosition).Length( );
            }

            spriteBatch.Begin( );

            for(int index = 0; index < text.Length; index++)
            { 
                
                if(transformedPosition[index].Z < 1 && distance[index] > 0 && height[index]*scale/distance[index] > 2 )
                { 
                    spriteBatch.DrawString(spriteFont,
                                            text[index],
                                            new Vector2(transformedPosition[index].X, transformedPosition[index].Y),                                 
                                            color,
                                            0,
                                            Vector2.Zero,
                                            scale/distance[index],
                                            SpriteEffects.None,
                                            0);
                }
            }
            spriteBatch.End( );

            graphics.BlendState = bs;
            graphics.DepthStencilState = ds;            
        }

        public SpriteFont SpriteFont
        {
            set { spriteFont = value; }
        }
    }
}
