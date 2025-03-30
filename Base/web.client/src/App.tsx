import { useEffect, useState } from 'react'
import './App.css'

function App() {
    const [tasks, setTasks] = useState([]);

    useEffect(() => {
        fetch('https://localhost:7020/api/task')
            .then(response => response.json())
            .then(data => setTasks(data))
    }, []);

  return (
      <div>
        <h3></h3>
      </div>
  )
}

export default App
