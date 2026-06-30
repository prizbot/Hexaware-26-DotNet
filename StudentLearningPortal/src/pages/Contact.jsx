import React from 'react'
import { useNavigate } from 'react-router-dom'

export default function Contact() {
  const navigate = useNavigate()

  return (
    <div className="page contact-page">
      <h1>Contact Us</h1>
      <div className="contact-card">
        <p><strong>Email:</strong> support@studentportal.com</p>
        <p><strong>Phone:</strong> 9876543210</p>
        <p><strong>Location:</strong> Chennai, India</p>
      </div>
      <button className="btn btn-secondary" onClick={() => navigate(-1)}>
        Go Back
      </button>
    </div>
  )
}
