import React from 'react'
import { useNavigate } from 'react-router-dom'
import courses from '../data/courses.js'

export default function Courses() {
  const navigate = useNavigate()

  return (
    <div className="page courses-page">
      <h1>Available Courses</h1>
      <div className="course-grid">
        {courses.map((course) => (
          <div className="course-card" key={course.id}>
            <h3>{course.title}</h3>
            <p><strong>Category:</strong> {course.category}</p>
            <p><strong>Duration:</strong> {course.duration}</p>
            <p><strong>Trainer:</strong> {course.trainer}</p>
            <button
              className="btn btn-primary"
              onClick={() => navigate(`/courses/${course.id}`)}
            >
              View Details
            </button>
          </div>
        ))}
      </div>
    </div>
  )
}
