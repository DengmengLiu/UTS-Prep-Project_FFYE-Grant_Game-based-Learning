# FFYE Grant Project (UTS Prep Guide)
**August 2024 - December 2024**   
[Demo](https://liudengmeng.itch.io/uts-perp)

## Project Overview
This project aims to help new students navigate key university processes, such as course enrolment, academic support, and timetable management, through an interactive game-based system. By leveraging game mechanics, the project enhances user engagement and improves the efficiency of information retention compared to traditional methods.

## Technical Implementation

### Game Development
- Built with **Unity**, utilizing:
  - **NavMesh** for pathfinding
  - **Cinemachine** for multi-camera control
  - **Canvas UI** for menu interactions

### Interactive Systems
- Implemented **Unity Animation** for player-NPC interactions, with additional animations sourced from Mixamo.
- Developed a quiz-based shooting system using **Scriptable Objects** to manage the question database.
- Employed **object pooling** to optimize balloon spawning and shooting mechanics.
- Integrated **particle systems** for explosion effects upon correct answers.
- **Embedded Video:** Used Unity’s built-in video player to ensure seamless integration within the game.

## User Testing & Feedback Optimization
- **Test Participants:** 10+ undergraduate and postgraduate students from UTS, with 12 detailed user feedback reports collected.
- **Key Improvements:**
  - Improved menu navigation by replacing flip-based interactions with button-based controls for better usability.
  - Added in-game tutorials at the beginning of each level to prevent player confusion.
  - Introduced navigation assistance to guide players towards their objectives.

## Impact & Results
- User feedback indicated that the game-based approach improved information retention by 20-45% compared to traditional university websites.
- **Deliverables:** A WebGL game deployed on itch.io.
- The project serves as a research case study at UTS, exploring the potential of games as an educational tool.

## Project Management & Collaboration
- **GitHub Version Control:** Independently managed 9 commits for version synchronization across multiple devices.
- **Agile Workflow:** Conducted weekly stakeholder meetings with the project supervisor to ensure tasks were on track.
- **Development Efficiency:** Reused game framework components to reduce redundant work, saving 8-10 hours of development time.
