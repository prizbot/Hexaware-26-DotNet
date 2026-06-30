import React from 'react'
import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext.jsx'

export default function Navbar() {
  const { isLoggedIn, user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const linkClass = ({ isActive }) => 'nav-link' + (isActive ? ' active' : '')

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <NavLink to="/" className="brand-link">
          🎓 Student Learning Portal
        </NavLink>
      </div>
      <div className="navbar-links">
        <NavLink to="/" end className={linkClass}>Home</NavLink>
        <NavLink to="/about" className={linkClass}>About</NavLink>
        <NavLink to="/courses" className={linkClass}>Courses</NavLink>
        <NavLink to="/contact" className={linkClass}>Contact</NavLink>

        {!isLoggedIn && (
          <NavLink to="/login" className={linkClass}>Login</NavLink>
        )}

        {isLoggedIn && (
          <>
            <NavLink to="/dashboard" className={linkClass}>Dashboard</NavLink>
            <span className="navbar-username">Hi, {user?.name || 'Student'}</span>
            <button className="btn btn-logout" onClick={handleLogout}>Logout</button>
          </>
        )}
      </div>
    </nav>
  )
}
