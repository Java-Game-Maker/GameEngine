import math

class PerlinNoise:
    def __init__(self, seed=0):
        self.perm = list(range(256))
        for i in range(256):
            self.perm.append(self.perm[i])
    
    def fade(self, t):
        return t * t * t * (t * (t * 6 - 15) + 10)
    
    def lerp(self, t, a, b):
        return a + t * (b - a)
    
    def grad(self, hash, x, y, z):
        h = hash & 15
        u = x if h < 8 else y
        v = y if h < 4 else (x if h in [12, 14] else z)
        return ((u if h & 1 == 0 else -u) + (v if h & 2 == 0 else -v))
    
    def noise(self, x, y, z):
        p = self.perm
        X = math.floor(x) & 255
        Y = math.floor(y) & 255
        Z = math.floor(z) & 255
        
        x -= math.floor(x)
        y -= math.floor(y)
        z -= math.floor(z)
        
        u = self.fade(x)
        v = self.fade(y)
        w = self.fade(z)
        
        A = p[X] + Y
        AA = p[A] + Z
        AB = p[A + 1] + Z
        B = p[X + 1] + Y
        BA = p[B] + Z
        BB = p[B + 1] + Z
        
        return self.lerp(w, self.lerp(v, self.lerp(u, self.grad(p[AA], x, y, z), self.grad(p[BA], x - 1, y, z)),
                                       self.lerp(u, self.grad(p[AB], x, y - 1, z), self.grad(p[BB], x - 1, y - 1, z))),
                          self.lerp(v, self.lerp(u, self.grad(p[AA + 1], x, y, z - 1), self.grad(p[BA + 1], x - 1, y, z - 1)),
                                    self.lerp(u, self.grad(p[AB + 1], x, y - 1, z - 1), self.grad(p[BB + 1], x - 1, y - 1, z - 1))))