import React from 'react'

export default function Settings() {
  return (
    <div className="dashboard-panel">
      <h2>Settings</h2>
      <div className="settings-section">
        <h3>Change Password</h3>
        <p>Update your account password for better security.</p>
      </div>
      <div className="settings-section">
        <h3>Notification Preferences</h3>
        <p>Manage how you receive notifications from the portal.</p>
      </div>
      <div className="settings-section">
        <h3>Theme Preferences</h3>
        <p>Choose between light and dark theme for your dashboard.</p>
      </div>
    </div>
  )
}
