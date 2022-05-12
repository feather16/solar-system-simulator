using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemSimulator
{
    public class Planet
    {
        public string name = "";
        public double mass = 0;
        public Color color = Color.Black;
        public Vector2 p = Vector2.ZERO;
        public Vector2 v = Vector2.ZERO;
        public Vector2 a = Vector2.ZERO;

        public double x
        {
            get => p.x;
            set => p.x = value;
        }
        public double y
        {
            get => p.y;
            set => p.y = value;
        }
        public double vx
        {
            get => v.x;
            set => v.x = value;
        }
        public double vy
        {
            get => v.y;
            set => v.y = value;
        }

        public Planet() { }
        /*public Planet(string name, double mass, 
            double x, double y)
        {
            this.name = name;
            this.mass = mass;
            p = new Vector2(x, y);
        }
        public Planet(string name, double mass, Vector2 p)
            : this(name, mass, p.x, p.y) { }*/
        
        public void appliedForce(Vector2 f)
        {
            // ma = f
            a += f / mass;
        }

        public void move(double dt)
        {
            v += a * dt;
            p += v * dt;
            a = Vector2.ZERO;
        }

        public double distanceTo(Planet planet)
        {
            return (p - planet.p).abs;
        }
    }
}
