import { useState, useEffect } from 'react'
import React from 'react'
import { getPeople } from '../api/personApi'
import CreateForm from './CreateForm'
import EditForm from './EditForm'
import DeleteConfirm from './DeleteConfirm'

function getBirthdays(people, sortBy = 'name', search = '') {
  const normalizedSearch = search.trim().toLowerCase();
  return [...people]
    .filter(p => {
      if (!normalizedSearch) return true;

      return (
        p.name.toLowerCase().includes(normalizedSearch) ||
        p.relationshipType.toLowerCase().includes(normalizedSearch)
      );
    }).sort((a, b) => {
    switch (sortBy.toLowerCase()) {
      case 'name':        
        return a.name.localeCompare(b.name);
      case 'age':
        return calculateAge(a.birthDate) - calculateAge(b.birthDate);
      case 'birthdate':
        return new Date(a.birthDate) - new Date(b.birthDate);
      case 'relationship':
        return a.relationshipType.localeCompare(b.relationshipType);
      default:
        return 0;
    }
  });
}

function calculateAge(birthDate) {
    const today = new Date();
    const birthdate = new Date(birthDate);
    var age = today.getFullYear() - birthdate.getFullYear();

    const birthdayThisyear = new Date(
      today.getFullYear(),
      birthdate.getMonth(),
      birthdate.getDate()
    );

    if (today < birthdayThisyear) {
      --age;
    }
    return age;
}

const AllBirthdays = () => {
    const [people, setPeople] = useState([]);
    const [sortBy, setSortBy] = useState('name');
    const [search, setSearch] = useState('');
    //CreateForm
    const [createForm, setCreateForm] = useState(false);
    //EditForm
    const [editForm, setEditForm] = useState(false);
    const [editingGuid, setEditingGuid] = useState(null);
    //DeleteForm
    const [deleteForm, setDeleteForm] = useState(false);
    const [deletingGuid, setDeletingGuid] = useState(null);

    const reloadPeople = () => {
      getPeople()
        .then(data => {
        const sorted = getBirthdays(data, sortBy, search);
        setPeople(sorted);
        })
        .catch(err => console.error(err));
      setCreateForm(false);
      setEditForm(false);
      setDeleteForm(false);
    };
  
    useEffect(() => {
      reloadPeople();
    }, [sortBy, search]);

  return (
    <main className='container'>
      <h1>All Birthdays</h1>      
      <div className='action-panel'> 
{/*CreateButton*/} 
        <button className='button' onClick={() => setCreateForm(!createForm)}>
          {createForm ? 'Close form' : 'Create person'}
        </button>      
{/*Sort*/}
        <label>
            Sort by:{' '}
            <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                <option value="name">Name</option>
                <option value="birthdate">Birthdate</option>
                <option value="age">Age</option>
                <option value="relationship">Relationship</option>
            </select>
        </label> 
{/*Search*/}
        <label>
          Search: {' '}
          <input 
            type="text" 
            placeholder="Search" 
            value={search} 
            onChange={e => setSearch(e.target.value)} 
          /> 
        </label>
      </div>
{/*CreateForm*/}
      {createForm && (
        <div className='create-form slide-down'>
          <h3>Create person</h3>
          <CreateForm onCreated={reloadPeople} onCancel={() => setCreateForm(false)} />
        </div>
      )}
{/*MainTable*/}
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Birthdate</th>
            <th>Age</th>
            <th>Relationship</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {people.map(p => (
            <React.Fragment key={p.guid}>
              <tr>
                <td>{p.name}</td>
                <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
                <td>{calculateAge(p.birthDate)}</td>
                <td>{p.relationshipType}</td>
                <td className="action-td">
                  <div className='image'>
                    <img
                      src="/edit.png"
                      alt="edit-icon"
                      onClick={() => {
                        setDeleteForm(false)
                        setEditForm(!editForm)
                        setEditingGuid(p.guid)
                        }
                      }
                    />
                    <img 
                      src="/delete.png" 
                      alt="delete-icon"
                      onClick={() => {
                        setEditForm(false)
                        setDeleteForm(!deleteForm)
                        setDeletingGuid(p.guid)
                      }
                      } 
                    />
                  </div>                  
                </td>
              </tr>
{/*EditForm*/}
              {editForm === true && editingGuid === p.guid && (
                <tr>
                  <td colSpan="5">
                    <EditForm
                      person={p}
                      onCancel={() => {
                        setEditingGuid(null)
                        setEditForm(false)
                      }}
                      onSaved={reloadPeople}
                    />
                  </td>
                </tr>
              )}
{/*DeleteForm*/}
              {deleteForm === true && deletingGuid === p.guid && (
                <tr>
                  <td colSpan="5">
                    <DeleteConfirm 
                      guid={p.guid}
                      name={p.name}                      
                      onDeleted={reloadPeople}
                      onCancel={() => {
                        setDeletingGuid(null)
                        setDeleteForm(false)
                      }}
                    />
                  </td>                  
                </tr>
              )}
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </main>
  )
}

export default AllBirthdays