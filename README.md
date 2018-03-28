# TerrainStudy

What it currently does:
* Instantiates a map based on source textures. Basically textures are information of where to place indexed objects.
* Instantiated map is in a layering system, so it can instantiate multiple objects on top of each other to create complex structures.
* Instantiated objects are merged into a single mesh.

TODO:

* Each voxel has a type. This type is used on several systems. A voxel type can be something like: Brick wall, Water, Wooden Floor, etc.
* We must be able to create seams between different types, so when Generator finds a type that has a valid seam already assigned, it uses it to fill the gap between multiple voxel types.
* Create seams, geometry between weldable vertices.
