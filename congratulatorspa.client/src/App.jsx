import './css/App.css'
import './css/Header.css'
import './css/Table.css'
import './css/CreateForm.css'
import './css/EditForm.css'
import './css/DeleteConfirm.css'
import { Routes, Route } from 'react-router-dom'
import Header from './components/Header'
import Home from './components/Home'
import AllBirthdays from './components/AllBirthdays'

const App = () => {
  return (
    <>
      <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/allbirthdays" element={<AllBirthdays />} />
      </Routes>
    </>
  )  
}

export default App
