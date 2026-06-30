import React from 'react'
import { useNavigate } from 'react-router-dom'

export default function Home() {
  const navigate = useNavigate()

  return (
    <div className="page home-page">
      <div className="hero">
        <h1>Welcome to Student Learning Portal</h1>
        <p className="hero-subtitle">
          Learn React, Web API, and Full Stack Development from one place.
        </p>
        <div className="hero-buttons">
          <button className="btn btn-primary" onClick={() => navigate('/courses')}>
            View Courses
          </button>
          <button className="btn btn-secondary" onClick={() => navigate('/dashboard')}>
            Go to Dashboard
          </button>
        </div>
      </div>
    </div>
  )
}
