# 🛒 Cashara Simulator

<p align="center">
  <img src="Cashara-Simulator-ScreenShots/cashara_simulator.jpg" alt="Cashara Simulator Cover Art" width="100%">
</p>

[![Unity](https://img.shields.io/badge/Unity-6.3%20LTS-black?style=flat&logo=unity)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20WebGL-blue)](https://play.unity.com)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A first-person 3D market management simulator built with **Unity 6**. Build shelves and registers, order products from the order panel, stock the shelves with the delivered boxes and let customers shop. Earn coins on every sale and grow your market!

🎮 **[Play the Game in Your Browser (Unity Play)](https://play.unity.com/en/games/d7a198f3-f7b0-4de5-9088-c90002795244/cashara-simulator)**

---

## 📸 Screenshots

*In-game screenshots:*

### Building

| Shelf (valid) | Shelf (blocked) | Register (valid) | Register (blocked) |
| :---: | :---: | :---: | :---: |
| ![Shelf valid](Cashara-Simulator-ScreenShots/CS_Building_Shelf.png) | ![Shelf blocked](Cashara-Simulator-ScreenShots/CS_Building_Shelf_No.png) | ![Register valid](Cashara-Simulator-ScreenShots/CS_Building_Cash.png) | ![Register blocked](Cashara-Simulator-ScreenShots/CS_Building_Cash_No.png) |

### Ordering

| Open Panel | Categories | Food | Drink |
| :---: | :---: | :---: | :---: |
| ![Interact](Cashara-Simulator-ScreenShots/CS_OrderPanel_Interact.png) | ![Order panel](Cashara-Simulator-ScreenShots/CS_OrderPanel.png) | ![Food](Cashara-Simulator-ScreenShots/CS_OrderPanel_Food.png) | ![Drink](Cashara-Simulator-ScreenShots/CS_OrderPanel_Drink.png) |

| Snack | Cart Summary | Max Order Warning | Order Received |
| :---: | :---: | :---: | :---: |
| ![Snack](Cashara-Simulator-ScreenShots/CS_OrderPanel_Snack.png) | ![Cart](Cashara-Simulator-ScreenShots/CS_OrderPanel_CartSummary.png) | ![Max order](Cashara-Simulator-ScreenShots/CS_OrderPanel_MaxOrder.png) | ![Order received](Cashara-Simulator-ScreenShots/CS_OrderPanel_OrderReceived.png) |

### Delivery, Shelves and Customers

| Order Box | Stocked Shelf | Customers at the Shelf |
| :---: | :---: | :---: |
| ![Order box](Cashara-Simulator-ScreenShots/CS_OrderBox_png.png) | ![Stocked shelf](Cashara-Simulator-ScreenShots/CS_Shelf_Product.png) | ![Shelf customers](Cashara-Simulator-ScreenShots/CS_Shelf_Customer.png) |

| Customers at the Register | Complete the Sale |
| :---: | :---: |
| ![Register customers](Cashara-Simulator-ScreenShots/CS_Cash_Customer.png) | ![Complete sale](Cashara-Simulator-ScreenShots/CS_Interact_Cash.png) |

### Pause Menu

| Pause | Settings |
| :---: | :---: |
| ![Pause](Cashara-Simulator-ScreenShots/CS_Pause.png) | ![Pause settings](Cashara-Simulator-ScreenShots/CS_Pause_Settings.png) |

---

## ✨ Features

- **Build Mode:** Toggle build mode, pick a shelf or a register, preview it with green (valid) / red (blocked) feedback, rotate it with the mouse wheel and place it. Placement is free.
- **Order Panel:** Browse Food, Drink and Snack categories, set quantities, review the cart and confirm the order. Warnings appear for insufficient funds and for exceeding the box capacity.
- **Order Boxes:** Each order arrives in a box with 9 slots (3x3). Pick it up, carry it to a shelf and the products jump onto the free slots (DOTween animations). Empty boxes are removed automatically.
- **Coin Economy:** Start with 200 coins. Products have separate buy and sell prices, so every sale makes a profit.
- **Customer AI:** Customers use NavMesh and a task-based system (patrol, visit a shelf, pay at the register, leave). They take 1-3 products and wait up to 5 seconds if a shelf is empty.
- **Queue System:** Shelves and registers manage their own customer queues; customers line up and move forward when someone leaves.
- **Register Interaction:** The first customer in line turns the register screen green; interact with it to complete the sale and earn coins.
- **Customer Spawner:** Several customer characters spawn over time up to a maximum count.
- **First-Person Controls:** Smooth WASD movement and mouse look with Cinemachine.
- **Pause Menu & Settings:** Pause, resume, restart and quit, with separate music and SFX sliders (Audio Mixer, saved with PlayerPrefs).
- **Audio System:** Music, build, shelf, pickup, drop, order, customer, money, UI and footstep sounds handled by a central AudioManager.
- **Endless Gameplay:** There is no end condition, the goal is to keep your market running.

---

## 🕹️ Controls

| Action | Input |
| ------ | ----- |
| **Move** | `W` `A` `S` `D` |
| **Look** | Mouse |
| **Toggle build mode** | `B` |
| **Select shelf / register** | `1` / `2` |
| **Rotate building** | Mouse wheel |
| **Place building** | Left mouse button |
| **Cancel building** | Right mouse button |
| **Interact** | `E` (order panel, order box, register screen; also drops a carried box) |
| **Pause** | `P` |

---

## 🧩 Scripts

| Script | Purpose |
| ------ | ------- |
| `LouiseController` / `InputManager` | First-person movement, camera control, raycast interaction, carrying items and Input System bindings |
| `BuildingManager` / `BaseBuilding` | Build mode, placement preview, validity check and placing |
| `BaseQueueBuilding` / `Shelf` / `Cash` / `CashScreen` | Queue logic, shelf slots and register interaction |
| `OrderPanel` / `ProductPanel` | Order UI, cart, cost check and box capacity check |
| `ProductManager` / `Product_Box` / `BaseProduct` | Order delivery, box slots and product data (buy / sell price) |
| `Product_Food` / `Product_Drink` / `Product_Snack` | Product categories |
| `BaseCustomer` / `CustomerSpawner` | Customer behavior and spawning |
| `BaseTask` / `Task_Patrol` / `Task_Shelf` / `Task_Cash` / `Task_Leave` | Task-based customer AI |
| `IInteractable` / `IAttachable` / `SlotSpecs` | Shared interfaces and slot data |
| `GameManager` / `UIManager` | Coins, placed buildings and HUD |
| `AudioManager` / `PauseManager` / `VolumeSettings` | Audio, pause menu and volume sliders |

---

## 🛠️ Tech Stack & Assets

- **Engine:** Unity 6.3 LTS (6000.3.13f1)
- **Language:** C#
- **Input:** Unity Input System
- **Camera:** Cinemachine
- **AI:** Unity NavMesh (AI Navigation)
- **Assets & Packages:**
  * Characters ([Mixamo](https://www.mixamo.com/)): Louise (player), Remy, Amy, Aj and Josh (customers)
  * Environment: [Fast Food Low Poly Building 3D](https://assetstore.unity.com/packages/3d/environments/urban/fast-food-low-poly-building-3d-180630)
  * Shelves: [Fresh Shelving](https://assetstore.unity.com/packages/3d/props/furniture/fresh-shelving-267101)
  * Products: [Food Pack Low Poly Assets](https://assetstore.unity.com/packages/3d/props/food/food-pack-low-poly-assets-323950), [Low Poly Cartoon Food and Groceries Pack](https://assetstore.unity.com/packages/3d/props/food/low-poly-cartoon-food-and-groceries-pack-98475)
  * UI: [371 Simple Buttons Pack](https://assetstore.unity.com/packages/2d/gui/icons/371-simple-buttons-pack-97516)
  * Animation: [DOTween (HOTween v2)](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
  * Music and sound effects: [Pixabay](https://pixabay.com/)

---

## 📁 Project Structure

```
Assets/
└── Development/
    ├── Animations/
    ├── AudioMixer/
    ├── Inputs/
    ├── Materials/
    ├── Models/
    ├── Prefabs/     → Customer, Monitor, Product, Shelf, Trash, UI
    ├── Scenes/
    ├── Script/
    ├── Sounds/
    └── Texture/
```

---

## 🚀 Getting Started (Local Setup)

1. **Clone the repository:**
```
   git clone https://github.com/emirsumer/CasharaSimulator.git
```
2. **Open with Unity:** Add the folder in Unity Hub and use Unity 6000.3.13f1 (or a compatible Unity 6 version).
3. **Run the Game:** Open `Assets/Development/Scenes/SampleScene.unity` and press **Play**.

---

## 🙏 Credits

- The core gameplay systems were developed together with my instructor.
- Characters from Mixamo; 3D models and UI from the Unity Asset Store; music and sound effects from Pixabay.
- Cover art: AI-generated.

## 📜 License

This project is open-source and available under the MIT License.
