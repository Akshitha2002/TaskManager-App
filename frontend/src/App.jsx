import { useState, useEffect } from 'react';
import axios from 'axios';

function App() {
  const [tasks, setTasks] = useState([]);
  const [newTaskTitle, setNewTaskTitle] = useState("");

  // IMPORTANT: Make sure 5216 matches the port your C# backend is running on!
  const API_URL = "https://mytaskmanagerapi.runasp.net/tasks";

  // Fetches tasks when the page loads
  useEffect(() => {
    axios.get(API_URL)
      .then(response => setTasks(response.data))
      .catch(error => console.error("Error fetching tasks:", error));
  }, []);

  // Sends a new task to the C# backend
  const handleAddTask = () => {
    if (!newTaskTitle) return; // Don't add empty tasks
    
    const newTask = { title: newTaskTitle, isCompleted: false };
    
    axios.post(API_URL, newTask)
      .then(response => {
        setTasks([...tasks, response.data]);
        setNewTaskTitle(""); // Clears the input box
      })
      .catch(error => console.error("Error adding task:", error));
  };

  return (
    <div style={{ padding: '30px', fontFamily: 'sans-serif', maxWidth: '500px', margin: '0 auto' }}>
      <h2>My Task Manager</h2>
      
      <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
        <input 
          type="text" 
          value={newTaskTitle} 
          onChange={(e) => setNewTaskTitle(e.target.value)} 
          placeholder="Type a new task here..."
          style={{ flexGrow: 1, padding: '8px', fontSize: '16px' }}
        />
        <button onClick={handleAddTask} style={{ padding: '8px 16px', fontSize: '16px', cursor: 'pointer' }}>
          Add Task
        </button>
      </div>

      <ul style={{ listStyle: 'none', padding: 0 }}>
        {tasks.map(task => (
          <li key={task.id} style={{ padding: '12px', borderBottom: '1px solid #eee', fontSize: '18px' }}>
            {task.title}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;