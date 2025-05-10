# ChunkSystem

**A lightweight, event-driven 2D chunk system for Unity.**

This system lets you dynamically load, enable, and disable world "chunks" based on agent positions (like a player), enabling modular level streaming and performance-friendly world management in large 2D games.

---

## 🚀 Getting Started

### 1. Set Up a Chunk Manager

Create an empty GameObject in your scene and add the `ChunkManager` component.

In the Inspector:

* Assign your **chunk prefab** (a GameObject representing one chunk).
* Configure the chunk size and update frequency.
* Optionally assign any `ChunkListeners` to respond to events.

> 💡 Chunks are defined by a `Bounds` object and automatically created/activated/deactivated based on agent proximity.

---

### 2. Add a Chunk Agent

To track a player or camera, attach a `ChunkAgent` component to their GameObject.

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        gameObject.AddComponent<ChunkAgent>();
    }
}
```

This tells the `ChunkManager` which parts of the world should stay loaded based on the agent's position.

---

### 3. Respond to Chunk Events

Use a `ChunkListener` or implement `IListenChunk` to handle lifecycle events:

---

#### 🧩 A. Using Code (Inherit from `ChunkListener`)

```csharp
using UnityEngine;

public class MyChunkLogger : ChunkListener
{
    public override void OnChunkCreated(Bounds bounds)
    {
        Debug.Log("Chunk created: " + bounds);
    }

    public override void OnChunkEnabled(Bounds bounds)
    {
        Debug.Log("Chunk enabled: " + bounds);
    }

    public override void OnChunkDisabled(Bounds bounds)
    {
        Debug.Log("Chunk disabled: " + bounds);
    }
}
```

Attach your listener script to any GameObject and assign it to the `ChunkManager`.

---

## ✅ Features

* Event-driven: hook into chunk creation, activation, and deactivation.
* Modular: supports multiple agents (player, camera, NPCs).
* Efficient: loads only necessary chunks based on agent locations.
* Debuggable: includes `ChunkDebugger` for visualization in the editor.
* Clean: decouples chunk logic from game-specific content.

---

## 📁 Folder Structure

```
ChunkSystem/
├── Chunk.cs
├── ChunkAgent.cs
├── ChunkListener.cs
├── IListenChunk.cs
├── ChunkManager.cs
└── ChunkDebugger.cs
```

---

## 🧪 Example Use Cases

* Streaming large 2D terrain without performance hitches.
* Activating enemy spawns or weather systems in relevant regions.
* Saving/loading world state per chunk.
* Creating procedural worlds with dynamic loading zones.

---

## 📌 Notes

* Chunks are identified by grid-aligned world positions and must all be the same size.
* You can extend `Chunk`, `ChunkAgent`, or `ChunkManager` for more complex logic.

---

## 🛠️ Future Ideas

* Add pooling support to minimize chunk instantiation overhead.
* Support for 3D worlds or vertical layers.
* Chunk prioritization based on agent type or importance.