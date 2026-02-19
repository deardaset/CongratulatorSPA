import { useState, useEffect } from 'react'
import React from 'react'
import { getPeople } from '../api/personApi'
import CreateForm from './CreateForm'
import EditForm from './EditForm'
import DeleteConfirm from './DeleteConfirm'

const AllBirthdays = () => {
    const [people, setPeople] = useState([]);
    //Pagination sort and search
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [totalCount, setTotalCount] = useState(0);
    const [sortBy, setSortBy] = useState('name');
    const [searchBy, setSearch] = useState('');
    const [upcoming] = useState(false);
    //Loading
    const [loading, setLoading] = useState(false);
    //CreateForm
    const [createForm, setCreateForm] = useState(false);
    //EditForm
    const [editForm, setEditForm] = useState(false);
    const [editingGuid, setEditingGuid] = useState(null);
    //DeleteForm
    const [deleteForm, setDeleteForm] = useState(false);
    const [deletingGuid, setDeletingGuid] = useState(null);

    const reloadPeople = () => {
      setLoading(true);
      getPeople({page, pageSize, sortBy, searchBy, upcoming})
        .then(data => {
        setPeople(data.data);
        setTotalCount(data.totalCount);
        })
        .catch(err => console.error(err))
        .finally(() => setLoading(false));        
      setCreateForm(false);
      setEditForm(false);
      setDeleteForm(false);
    };
  
    useEffect(() => {
      reloadPeople();
    }, [page, sortBy, searchBy]);

    const totalPages = Math.ceil(totalCount / pageSize);

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
                <option value="nextbirthday">Nextbirthday</option>
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
            value={searchBy} 
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
      {loading ? (
        <div className="skeleton-row"></div>
      ) : (
      <table>
        <thead>
          <tr>
            <th>Photo</th>
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
                <td className='avatar'>
                  <img src={p.photoUrl} alt="user-photo" />
                </td>
                <td>{p.name}</td>
                <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
                <td>{p.age}</td>
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
                  <td colSpan="6">
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
                  <td colSpan="6">
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
      )}
      <div className="pagination">
        <button
          className="icon-button"
          disabled={page === 1}
          onClick={() => setPage(p => p - 1)}
        >
          <img src="/left-arrow.png" alt="Previous page" />
        </button>

        <span>{page} / {totalPages}</span>

        <button
          className="icon-button"
          disabled={page === totalPages}
          onClick={() => setPage(p => p + 1)}
        >
          <img src="/right-arrow.png" alt="Next page" />
        </button>
      </div>
    </main>
  )
}

export default AllBirthdays