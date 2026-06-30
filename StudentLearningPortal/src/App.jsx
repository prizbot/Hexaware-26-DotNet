import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Navbar from './components/Navbar.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'

import Home from './pages/Home.jsx'
import About from './pages/About.jsx'
import Courses from './pages/Courses.jsx'
import CourseDetails from './pages/CourseDetails.jsx'
import Contact from './pages/Contact.jsx'
import Login from './pages/Login.jsx'
import NotFound from './pages/NotFound.jsx'

import Dashboard from './pages/dashboard/Dashboard.jsx'
import Profile from './pages/dashboard/Profile.jsx'
import MyCourses from './pages/dashboard/MyCourses.jsx'
import Settings from './pages/dashboard/Settings.jsx'

export default function App() {
  return (
    <>
      <Navbar />
      <main className="main-content">
        <Routes>
          {/* Public routes */}
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/courses" element={<Courses />} />
          <Route path="/courses/:id" element={<CourseDetails />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/login" element={<Login />} />

          {/* Protected routes with nested children */}
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="profile" replace />} />
            <Route path="profile" element={<Profile />} />
            <Route path="my-courses" element={<MyCourses />} />
            <Route path="settings" element={<Settings />} />
          </Route>

          {/* Wildcard / 404 */}
          <Route path="*" element={<NotFound />} />
        </Routes>
      </main>
    </>
  )
}
