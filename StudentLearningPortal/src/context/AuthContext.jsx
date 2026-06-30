import React, { createContext, useContext, useState } from 'react'

const AuthContext = createContext(null)

const DUMMY_USERNAME = 'student'
const DUMMY_PASSWORD = 'student123'

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem('slp_user')
    return stored ? JSON.parse(stored) : null
  })

  const login = (username, password) => {
    if (username === DUMMY_USERNAME && password === DUMMY_PASSWORD) {
      const userData = { username, name: 'Student User', email: 'student@example.com' }
      localStorage.setItem('slp_user', JSON.stringify(userData))
      localStorage.setItem('slp_loggedIn', 'true')
      setUser(userData)
      return { success: true }
    }
    return { success: false, message: 'Invalid username or password' }
  }

  const logout = () => {
    localStorage.removeItem('slp_user')
    localStorage.removeItem('slp_loggedIn')
    setUser(null)
  }

  const isLoggedIn = !!user || localStorage.getItem('slp_loggedIn') === 'true'

  return (
    <AuthContext.Provider value={{ user, login, logout, isLoggedIn }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}
