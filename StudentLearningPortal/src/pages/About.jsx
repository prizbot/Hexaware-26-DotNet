import React from 'react'

export default function About() {
  return (
    <div className="page about-page">
      <h1>About Student Learning Portal</h1>
      <p>
        This Student Learning Portal helps students view available courses, access
        their dashboard, manage their profile, and track enrolled courses.
      </p>

      <h2>Main Features</h2>
      <ul className="feature-list">
        <li>Browse all available courses without logging in</li>
        <li>View detailed information for each course</li>
        <li>Secure login system for registered students</li>
        <li>Personalized dashboard for logged-in students</li>
        <li>Profile management and enrolled course tracking</li>
        <li>Customizable account settings</li>
      </ul>
    </div>
  )
}
