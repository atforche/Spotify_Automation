using NLog;
using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

/// <summary>
/// Base class representing functionality shared by all API models
/// </summary>
public abstract class BaseModel
{
    /// <summary>
    /// Logger specific to this class
    /// </summary>
    protected Logger Logger;

    /// <summary>
    /// Validation error message for this particular model
    /// </summary>
    public abstract string ValidationErrorMessage { get; }

    /// <summary>
    /// Constructs a BaseModel object. Initializes the Logger that is unique to this class
    /// </summary>
    public BaseModel()
    {
        Logger = LogManager.GetLogger(GetType().Name);
    }

    /// <summary>
    /// Validates that the given model object is valid.
    /// </summary>
    /// <returns>True if the model object is valid, false otherwise</returns>
    protected abstract bool ValidatePrivate();

    /// <summary>
    /// Validates that the given model object is valid. Also logs any validation failures
    /// </summary>
    /// <returns>True if the model object is valid, false otherwise</returns>
    public bool Validate()
    {
        if (!ValidatePrivate())
        {
            Logger.Error(ValidationErrorMessage);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Validates that the given model object is valid. If validation throws an error,
    /// catch that error and return false
    /// </summary>
    /// <returns>True if the model object is valid, false if the model is invalid or the validation fails in error</returns>
    public bool TryValidate()
    {
        try
        {
            return Validate();
        }
        catch (Exception e)
        {
            Logger.Error($"Error validating model. Error: {e}");
            return false;
        }
    }

    /// <summary>
    /// Validates that the given model object is valid, otherwise throws an error 
    /// </summary>
    public void ValidateOrError()
    {
        if (!Validate())
        {
            var exception = new Exception(ValidationErrorMessage);
            throw exception;
        }
    }
}

/// <summary>
/// Static class containing extension methods that operate on objects of type BaseModel
/// </summary>
public static class BaseModelExtensions
{
    /// <summary>
    /// Logger specific to this class
    /// </summary>
    public static Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Validates that the given model object is valid and not null
    /// </summary>
    /// <param name="model">The BaseModel object to validate</param>
    /// <returns>True if the model object is valid and not null, false otherwise</returns>
    public static bool ValidateNull([NotNullWhen(true)] this BaseModel? model) => model == null ? throw new ArgumentNullException() : model.Validate();

    /// <summary>
    /// Validates that the given model object is valid and not null, otherwise throws an error 
    /// </summary>
    /// <param name="model">The BaseModel object to validate</param>
    public static void ValidateOrErrorNull([NotNull] this BaseModel? model)
    {
        if (model == null)
        {
            var exception = new ArgumentNullException();
            Logger.Error(exception);
            throw exception;
        }
        else
        {
            model.ValidateOrError();
        }
    }
}
