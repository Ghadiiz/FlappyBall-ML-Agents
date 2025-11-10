# FlappyBall ML-Agents

A FlappyBird-style game where an AI agent learns to navigate through pipe obstacles using Unity ML-Agents and Proximal Policy Optimization (PPO).

## 🎮 Project Overview

This project demonstrates reinforcement learning in action. A ball-shaped agent learns to:
- Navigate through procedurally generated pipe obstacles
- Avoid ground, ceiling, and walls
- Maximize survival time through trial and error

**Peak Performance:** Cumulative reward of 95.620 after 300,000 training steps

## 🛠️ Technologies

- **Unity 2D** - Game engine and environment
- **Unity ML-Agents Toolkit v2.0.2** - Reinforcement learning framework
- **Python 3.9 + PyTorch 1.7.1** - Training backend
- **PPO Algorithm** - Proximal Policy Optimization

## 📊 Training Results

Three configurations were tested to compare hyperparameter impact:

| Config | Peak Reward | Training Time | Network | Description |
|--------|-------------|---------------|---------|-------------|
| **Config 1 (Baseline)** | **95.620** | 1.27 hr | 128 units, 2 layers | Standard PPO - Best overall |
| Config 2 (Higher LR) | 35.218 | 1.15 hr | 128 units, 2 layers | Faster but lower peak |
| Config 3 (Deeper Net) | 64.131 | 0.75 hr | 256 units, 3 layers | Fast learning |

**Winner:** Configuration 1 (Baseline) achieved the highest performance when trained to completion.

## 🚀 Getting Started

### Prerequisites

- Unity 2022 LTS or later
- Python 3.9
- Anaconda (recommended)

### Installation

1. **Clone this repository:**
https://github.com/Ghadiiz/FlappyBall-ML-Agents.git

2. **Open in Unity Hub**

3. **Install ML-Agents Python package:**
pip install mlagents==0.30.0

### Training Your Own Agent

1. **Navigate to Training folder:**
cd Assets/Training

2. **Start training:**
mlagents-learn FlappyBall.yaml --run-id=MyTraining

3. **Press Play in Unity** when prompted

4. **Monitor progress with TensorBoard:**
tensorboard --logdir results
Open browser: `http://localhost:6006`

### Using Pre-Trained Models

1. Navigate to `Assets/TrainedModels/`
2. Drag `.onnx` file into Ball agent's **Model** field in Unity Inspector
3. Set **Behavior Type** to **"Inference Only"**
4. Press Play to watch the trained agent!

## 🎯 Agent Details

### Observations (34 values)
- Vertical velocity (normalized)
- Vertical position (normalized)
- Ray perception data (8 rays × 4 tags = 32 values)

### Actions (Discrete)
- **0:** Do Nothing
- **1:** Jump

### Rewards
- **Collision:** -1.0 (obstacle/boundary)
- **Timestep:** -0.001 (efficiency pressure)
- **Survival:** Implicit (longer survival = higher cumulative reward)

## 🏆 Key Features

- **Ray Perception Sensor:** 8-ray system with 180° forward vision
- **Dynamic Pipe Generation:** Procedurally scaled obstacles
- **Parallel Training:** 3 simultaneous environments for 3× faster learning
- **Automatic Episode Management:** Reset on collision
- **TensorBoard Integration:** Real-time training visualization

## 👤 Author

**Ghadi Dababneh**  
(https://github.com/Ghadiiz)

## 🙏 Acknowledgments

- Unity ML-Agents Toolkit team
- PPO algorithm by Schulman et al.
- Course instructors and teaching assistants

## 📄 License

This project is for educational purposes. Academic use only.

---

**⭐ If you found this project helpful, please consider giving it a star!**
