import React from 'react'
import { NavLink, Outlet } from 'react-router-dom'

export default function Dashboard() {
  const linkClass = ({ isActive }) => 'dashboard-link' + (isActive ? ' active' : '')

  return (
    <div className="page dashboard-page">
      <h1>Welcome to Student Dashboard</h1>
      <div className="dashboard-layout">
        <aside className="dashboard-sidebar">
          <NavLink to="profile" className={linkClass}>Profile</NavLink>
          <NavLink to="my-courses" className={linkClass}>My Courses</NavLink>
          <NavLink to="settings" className={linkClass}>Settings</NavLink>
        </aside>
        <section className="dashboard-content">
          <Outlet />
        </section>
      </div>
    </div>
  )
}
