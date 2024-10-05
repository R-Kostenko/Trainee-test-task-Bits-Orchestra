using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using Trainee_Test.Data;
using Trainee_Test.Models;

namespace Trainee_Test.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DBContext _context;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, DBContext dBContext, IMapper mapper)
    {
        _logger = logger;
        _context = dBContext;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        List<PersonEntity> persons = await _context.Persons.ToListAsync();

        var personDTOs = _mapper.Map<List<PersonDTO>>(persons);

        return View(personDTOs);
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (id > 0)
        {
            var personRecord = await _context.Persons.FindAsync(id);
            var personDTO = _mapper.Map<PersonDTO>(personRecord);

            if (personRecord != null && personDTO != null)
            {
                return View(personDTO);
            }
        }
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        return View(new PersonDTO());
    }


    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        string message = string.Empty;

        if (file != null && file.Length > 0)
        {
            if (Path.GetExtension(file.FileName).ToLower() == ".csv")
            {
                try
                {
                    var csvDataList = new List<PersonCsvRow>();

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true
                    }))

                    csvDataList = csv.GetRecords<PersonCsvRow>().ToList();
                    var validatedData = csvDataList.Where(row =>
                    {
                        var validationContext = new ValidationContext(row);
                        var results = new List<ValidationResult>();
                        return Validator.TryValidateObject(row, validationContext, results, true);
                    });

                    var personsData = _mapper.Map<List<PersonEntity>>(validatedData);

                    if (validatedData.Any() && personsData != null && personsData.Count > 0)
                    {
                        _context.RemoveRange(_context.Persons);
                        _context.Persons.AddRange(personsData);
                        await _context.SaveChangesAsync();
                    }

                    message = "File uploaded and data saved successfully!";
                }
                catch (Exception ex)
                {
                    message = $"Error: {ex.Message}";
                }
            }
            else
            {
                message = "Please upload a valid CSV file.";
            }
        }
        else
        {
            message = "CSV file wasn't uploaded.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        string message = string.Empty;

        if (id > 0)
        {
            var personRecord = await _context.Persons.FindAsync(id);
            if (personRecord != null)
            {
                _context.Entry(personRecord).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                message = $"Record with ID: {id} has been successfully deleted!";
            }
            else
            {
                message = $"Record with ID: {id} wasn't found.";
            }
        }
        else
        {
            message = "Invalid ID was provided.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PersonDTO personDTO)
    {
        string message = string.Empty;

        if (ModelState.IsValid && personDTO.Id != 0)
        {
            if (await _context.Persons.AnyAsync(p => p.Id == personDTO.Id))
            {
                _context.Update(_mapper.Map<PersonEntity>(personDTO));
                await _context.SaveChangesAsync();
            }
            else
            {
                message = $"Record with ID: {personDTO.Id} wasn't found.";
            }
        }
        else
        {
            message = "Model is not valid.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonDTO personDTO)
    {
        string message = string.Empty;

        if (ModelState.IsValid && personDTO.Id == 0)
        {
            if (await _context.Persons.FirstOrDefaultAsync(p => p.Phone == personDTO.Phone) == null)
            {
                var personEntity = _mapper.Map<PersonEntity>(personDTO);
                await _context.Persons.AddAsync(personEntity);
                await _context.SaveChangesAsync();

                message = $"Record with Id: {personEntity.Id} had been successfully added!";
            }
            else
            {
                message = $"Record with Phone: {personDTO.Phone} already exists.";
            }
        }
        else
        {
            message = "Model is not valid.";
        }

        return RedirectToAction("Index");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}