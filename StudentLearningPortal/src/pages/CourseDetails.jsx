import React from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import courses from '../data/courses.js'

export default function CourseDetails() {
  const { id } = useParams()
  const navigate = useNavigate()

  const course = courses.find((c) => c.id === id)

  if (!course) {
    return (
      <div className="page course-details-page">
        <h1>Course not found</h1>
        <button className="btn btn-secondary" onClick={() => navigate('/courses')}>
          Back to Courses
        </button>
      </div>
    )
  }

  return (
    <div className="page course-details-page">
      <h1>Course Details</h1>
      <div className="course-details-card">
        <p><strong>Course ID:</strong> {course.id}</p>
        <p><strong>Title:</strong> {course.title}</p>
        <p><strong>Category:</strong> {course.category}</p>
        <p><strong>Duration:</strong> {course.duration}</p>
        <p><strong>Trainer:</strong> {course.trainer}</p>
        <p><strong>Description:</strong> {course.description}</p>
      </div>
      <button className="btn btn-secondary" onClick={() => navigate('/courses')}>
        Back to Courses
      </button>
    </div>
  )
}
