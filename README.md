# ChunkSystem
Reusable 2D chunk system in Unity

### Chunk
Base chunk class with helper functions

### ChunkManager
The ChunkManager triggers 3 events passing a Bounds in arguments:
- onChunkCreated
- onChunkEnabled
- onChunkDisabled

### ChunkAgent
- Add this component to any entity that will trigger the ChunkManager UnityEvents
    - Players, vehicles, idk anything really

### IChunkHandler
- Either chose to implement the IChunkHandler interface 
- Or drag and drop your own handler functions to the ChunkManager UnityEvents
- The ChunkManager will automatically find all IChunkHandler in scene
- With the handlers, you will be in charge to Instantiate, Enable or Disable things when events are fired