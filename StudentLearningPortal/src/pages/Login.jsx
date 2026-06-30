import React, { useState } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../context/AuthContext.jsx'

export default function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const from = location.state?.from?.pathname || '/dashboard'

  const handleSubmit = (e) => {
    e.preventDefault()
    setError('')

    if (!username.trim() && !password.trim()) {
      setError('Username and password are required')
      return
    }
    if (!username.trim()) {
      setError('Username is required')
      return
    }
    if (!password.trim()) {
      setError('Password is required')
      return
    }

    const result = login(username, password)
    if (result.success) {
      navigate(from, { replace: true })
    } else {
      setError(result.message)
    }
  }

  return (
    <div className="page login-page">
      <div className="login-card">
        <h1>Login</h1>
        <p className="login-hint">
          Demo credentials — Username: <strong>student</strong> / Password: <strong>student123</strong>
        </p>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Enter username"
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter password"
            />
          </div>

          {error && <div className="error-message">{error}</div>}

          <button type="submit" className="btn btn-primary btn-block">
            Login
          </button>
        </form>
      </div>
    </div>
  )
}
